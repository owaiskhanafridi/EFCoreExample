using EFCoreExample.Commands;
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
        public ItemController(ItemService svc, ILogger<ItemController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateItem(CreateItemCommand item, CancellationToken ct)
        {
            var entity = await _svc.CreateItemAsync(item, ct);

            return CreatedAtAction(nameof(GetById), new { entity.Id }, item);
        }

        [HttpGet("{id:guid}")]
        [EnableRateLimiting("fixed")]

        public async Task<ActionResult<Item>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _svc.GetItemAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet]
        [OutputCache]
        public async Task<ActionResult<List<Item>>> GetAllItems(CancellationToken ct)
        {
            _logger.LogInformation($"GetAllItems Method Called at {DateTime.Now}");
            await Task.Delay(3000);
            var items = await _svc.GetAllItemAsync(ct);
            return Ok(items);
        }
    }
}
