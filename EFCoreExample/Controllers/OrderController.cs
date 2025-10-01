using EFCoreExample.Models;
using EFCoreExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExample.Controllers
{
    [ApiController]
    [Route("/api")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _svc;
        public OrderController(OrderService svc) => _svc = svc;

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder(Order orderdto, CancellationToken ct)
        {
            var id = await _svc.CreateOrderAsync(orderdto, ct);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetById(Guid id, CancellationToken ct)
        {
            var order = await _svc.GetOrderAsync(id, ct);
            return order is null ? NotFound() : Ok(order);
        }
    }
}
