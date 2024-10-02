using Moq;
using System.Collections.Generic;
using MovEstApi.Controllers;
using Service;
using Infrastructure.DataModels;
using Microsoft.Extensions.Logging;
using MovEstApi.TransferModels;

namespace Tests.MovementsTests
{
    [TestClass]
    public class MovementsControllerTests
    {
        private readonly Mock<MovementService> _mockMovementService;
        private readonly Mock<ILogger<MovementsController>> _mockLogger;
        private readonly MovementsController _controller;

        public MovementsControllerTests()
        {
            _mockMovementService = new Mock<MovementService>();
            _mockLogger = new Mock<ILogger<MovementsController>>();
            _controller = new MovementsController(_mockLogger.Object, _mockMovementService.Object);
        }

        [TestMethod]
        public void GetAllMovements_ReturnsListOfMovements_WhenMovementServiceReturnsListOfMovements()
        {
            // Arrange
            var movements = new List<Movement> { new Movement(), new Movement() };
            _mockMovementService.Setup(ms => ms.GetMovementsForFeed()).Returns(movements);

            // Act
            var result = _controller.GetAllMovements();

            // Assert
            Assert.AreEqual(movements, result);
        }

        [TestMethod]
        public void GetAllMovements_ReturnsEmptyList_WhenMovementServiceReturnsEmptyList()
        {
            // Arrange
            var movements = new List<Movement>();
            _mockMovementService.Setup(ms => ms.GetMovementsForFeed()).Returns(movements);

            // Act
            var result = _controller.GetAllMovements();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetAllMovements_ThrowsException_WhenMovementServiceThrowsException()
        {
            // Arrange
            _mockMovementService.Setup(ms => ms.GetMovementsForFeed()).Throws<Exception>();

            // Act
            _controller.GetAllMovements();
        }

        [TestMethod]
        public void CreateMovement_ReturnsResponseDto_WhenMovementServiceCreatesMovement()
        {
            // Arrange
            var createMovementRequestDto = new CreateMovementRequestDto { Product_Sku = "sku", MovementType = 1, Quantity = 10 };
            var movement = new Movement();
            _mockMovementService.Setup(ms => ms.CreateMovement(createMovementRequestDto.Product_Sku, createMovementRequestDto.MovementType, createMovementRequestDto.Quantity)).Returns(movement);

            // Act
            var result = _controller.CreateProduct(createMovementRequestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Movement created successfully", result.Message);
            Assert.AreEqual(movement, result.ResponseData);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateMovement_ThrowsException_WhenMovementServiceThrowsException()
        {
            // Arrange
            var createMovementRequestDto = new CreateMovementRequestDto { Product_Sku = "sku", MovementType = 1, Quantity = 10 };
            _mockMovementService.Setup(ms => ms.CreateMovement(createMovementRequestDto.Product_Sku, createMovementRequestDto.MovementType, createMovementRequestDto.Quantity)).Throws<Exception>();

            // Act
            _controller.CreateProduct(createMovementRequestDto);
        }

        [TestMethod]
        public void DeleteMovement_ReturnsResponseDto_WhenMovementServiceDeletesMovement()
        {
            // Arrange
            var movementId = Guid.NewGuid();
            var productSku = "sku";
            var product = new Product();
            _mockMovementService.Setup(ms => ms.DeleteMovement(movementId, productSku)).Returns(product);

            // Act
            var result = _controller.DeleteMovement(movementId, productSku);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Movement deleted successfully", result.Message);
            Assert.AreEqual(product, result.ResponseData);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteMovement_ThrowsException_WhenMovementServiceThrowsException()
        {
            // Arrange
            var movementId = Guid.NewGuid();
            var productSku = "sku";
            _mockMovementService.Setup(ms => ms.DeleteMovement(movementId, productSku)).Throws<Exception>();

            // Act
            _controller.DeleteMovement(movementId, productSku);
        }
    }
}