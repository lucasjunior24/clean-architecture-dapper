using Xunit;
using Moq;
using DapperCrudClean.Application.Interfaces;
using DapperCrudClean.Application.UseCases;
using DapperCrudClean.Domain.Entities;

namespace DapperCrud.Test
{

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
        public async Task CreateShouldReturnNewProductId()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(1);

            var prod = new Product
            {
                Name = "Notebook",
                Stock = 100,
                Price = 10
            };
            // Act
            var result = await _service.Create(prod);

            // Assert
            Assert.Equal(1, result);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdShouldReturnProductWhenExists()
        {
            // Arrange
            Product product = new()
            {
                Name = "Mouse",
                Stock = 100,
                Price = 20
            };

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
        public async Task UpdateShouldReturnFalseWhenProductNotExists()
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
        public async Task UpdateShouldReturnTrueWhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Name = "Teclado",
                Stock = 150,
                Price = 5
            };

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
        public async Task DeleteShouldCallRepository()
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
}