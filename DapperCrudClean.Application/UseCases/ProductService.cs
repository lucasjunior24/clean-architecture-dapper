using DapperCrudClean.Domain.Entities;
using DapperCrudClean.Application.Interfaces;

namespace DapperCrudClean.Application.UseCases;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        var data = await _repository.GetAllAsync();
        return data;
        
    }

    public async Task<Product?> GetById(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<int> Create(Product product)
    {
        return await _repository.AddAsync(product);
    }

    public async Task<bool> Update(int id, string name, decimal price, int stock)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return false;

        product.Update(name, price, stock);
        return await _repository.UpdateAsync(product);
    }

    public async Task<bool> Delete(int id)
        => await _repository.DeleteAsync(id);
}