using Moq;
using System.Collections.Generic;
using MovEstApi.Controllers;
using Service;
using Infrastructure.DataModels;
using Microsoft.Extensions.Logging;

namespace Tests.ProductTests
{

    [TestClass]
    public class ProductsControllerTests
    {
        private readonly Mock<ProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<ProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockLogger.Object, _mockProductService.Object);
        }

        [TestMethod]
        public void GetAllProducts_ReturnsListOfProducts_WhenProductServiceReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product(), new Product() };
            _mockProductService.Setup(ps => ps.GetAllProducts()).Returns(products);

            // Act
            var result = _controller.GetAllProducts();

            // Assert
            Assert.AreEqual(products, result);
        }
        [TestMethod]
        public void GetAllProducts_ReturnsEmptyList_WhenProductServiceReturnsEmptyList()
        {
            // Arrange
            var products = new List<Product>();
            _mockProductService.Setup(ps => ps.GetAllProducts()).Returns(products);

            // Act
            var result = _controller.GetAllProducts();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetAllProducts_ThrowsException_WhenProductServiceThrowsException()
        {
            // Arrange
            _mockProductService.Setup(ps => ps.GetAllProducts()).Throws<Exception>();

            // Act
            _controller.GetAllProducts();
        }
    }
}