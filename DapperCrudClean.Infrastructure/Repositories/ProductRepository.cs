using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using DapperCrudClean.Application.Interfaces;
using DapperCrudClean.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace DapperCrud.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var db = Connection;
        var data =  await db.QueryAsync<Product>("SELECT * FROM Products");
        return data;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var db = Connection;
        return await db.QueryFirstOrDefaultAsync<Product>(
            "SELECT * FROM Products WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> AddAsync(Product product)
    {
        using var db = Connection;

        var sql = @"
            INSERT INTO Products (Name, Price, Stock)
            VALUES (@Name, @Price, @Stock);
            SELECT CAST(SCOPE_IDENTITY() as int);
        ";

        return await db.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        using var db = Connection;

        var sql = @"
            UPDATE Products
            SET Name = @Name,
                Price = @Price,
                Stock = @Stock
            WHERE Id = @Id
        ";

        return await db.ExecuteAsync(sql, product) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var db = Connection;
        return await db.ExecuteAsync(
            "DELETE FROM Products WHERE Id = @Id",
            new { Id = id }) > 0;
    }
}