using EFCoreExample.Infrastructure;
using EFCoreExample.Models;
using EFCoreExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExample.Controllers
{
    [ApiController]
    [Route("/api/v1/item")]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;
        private readonly AmazonDbContext _dbContext;
        public TestController(TestService testService, AmazonDbContext dbContext)
        {
            _testService = testService;
            _dbContext = dbContext;
        }

        [HttpGet("get-item-by-price")]
        public  IAsyncEnumerable<Item> GetItemsByPriceGreaterThan(decimal price)
        {
            return _testService.TestFuncDelegate(price);
        }

        [HttpGet("get-specific-timezone")]
        public string CreateDateWithSpecificTimeZone()
        {
            return _testService.CreateDateWithSpecificTimeZone();
        }

    }
}
