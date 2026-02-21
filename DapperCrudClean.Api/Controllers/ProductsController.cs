using DapperCrudClean.Application.UseCases;
using DapperCrudClean.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace DapperCrud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var product = await _service.GetAll();
        return product == null ? NotFound() : Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetById(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product request)
    {
        var id = await _service.Create(request);
        return Created("", new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductRequest request)
    {
        var updated = await _service.Update(id, request.Name, request.Price, request.Stock);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.Delete(id) ? NoContent() : NotFound();
}

public record ProductRequest(string Name, decimal Price, int Stock);