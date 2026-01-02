using EFCoreExample.Commands;
using EFCoreExample.CustomModelBinders;
using EFCoreExample.Models;
using EFCoreExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;
using System.Text.Json;

namespace EFCoreExample.Controllers
{
    [ApiController]
    [Route("/api/v1/item")]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.2")]

    public class ItemController : ControllerBase
    {
        private readonly ItemService _svc;
        private readonly ILogger<ItemController> _logger;
        private readonly TestService _testService;
        public ItemController(ItemService svc, ILogger<ItemController> logger, TestService tService)
        {
            _svc = svc;
            _logger = logger;
            _testService = tService;
        }

        [HttpGet("test-method")]
        public string TestMethod()
        {
            var result = _testService.DelegatePractice();
            return result.ToString();
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateItem(CreateItemCommand item, CancellationToken ct)
        {
            var entity = await _svc.CreateItemAsync(item, ct);

            return CreatedAtAction(nameof(GetById), new { entity.Id }, item);
        }

        /// <summary>
        /// Get Item by Id
        /// </summary>
        /// <param name="id"> a unique identified (guid)</param>
        /// <param name="ct"></param>
        /// <returns>item</returns>
        [HttpGet("{id:guid}")]
        [EnableRateLimiting("fixed")]

        public async Task<ActionResult<Item>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _svc.GetItemAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Gets all items.</summary>
        /// <remarks>Returns a paged list of items.</remarks>
        /// <response code="200">Items returned successfully.</response>
        /// <response code="404">Items not found.</response>
        /// <response code="500">Something went wrong.</response>

        [HttpGet]
        [OutputCache]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<List<Item>>> GetAllItems(CancellationToken ct)
        {
            _logger.LogInformation($"GetAllItems Method Called at {DateTime.Now}");
            await Task.Delay(3000);
            var items = await _svc.GetAllItemAsync(ct);
            return Ok(items);
        }

        [HttpGet("{title}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Item>> GetByItemTitle(string title, CancellationToken ct)
        {
            var item = await _svc.GetItemByTitleAsync(title);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("after-date")]
        public async Task<ActionResult<List<Item>>> GetItemsAddedAfter(
            [ModelBinder(BinderType = typeof(DateOnlyYyyyMmDdBinder))] DateOnly date, 
            CancellationToken ct)
        {
            DateTime dt = date.ToDateTime(TimeOnly.MinValue);
            var items = await _svc.GetItemsAddedAfter(dt);
            return Ok(items);
        }
    }
}
