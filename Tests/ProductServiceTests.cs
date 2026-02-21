using Xunit;
using Moq;
using DapperCrud.Application.Interfaces;
using DapperCrud.Application.UseCases;
using DapperCrud.Domain.Entities;

namespace DapperCrud.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnNewProductId()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(1);

        var prod = new Product();
        prod.Name = "Notebook";
        prod.Stock = 100;
        prod.Price = 10;
        // Act
        var result = await _service.Create(prod);

        // Assert
        Assert.Equal(1, result);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var product = new Product();
        product.Name = "Mouse";
        product.Stock = 100;
        product.Price = 20;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mouse", result!.Name);
    }

    [Fact]
    public async Task Update_ShouldReturnFalse_WhenProductNotExists()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.Update(1, "Teclado", 200m, 5);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Update_ShouldReturnTrue_WhenProductExists()
    {
        // Arrange
        var product = new Product();
        product.Name = "Teclado";
        product.Stock = 150;
        product.Price = 5;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(product);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.Update(1, "Teclado Gamer", 300m, 3);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldCallRepository()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}