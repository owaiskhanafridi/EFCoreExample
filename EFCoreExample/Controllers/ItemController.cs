﻿using EFCoreExample.Models;
using EFCoreExample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EFCoreExample.Controllers
{
    [ApiController]
    [Route("/api/item")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _svc;
        public ItemController(ItemService svc) => _svc = svc;

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateItem(Item itemDto, CancellationToken ct)
        {
            var item = await _svc.CreateItemAsync(itemDto, ct);

            return CreatedAtAction(nameof(GetById), new { item.Id }, item);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Item>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _svc.GetItemAsync(id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAllItems(CancellationToken ct)
        {
            var items = await _svc.GetAllItemAsync(ct);
            return Ok(items);
        }
    }
}
