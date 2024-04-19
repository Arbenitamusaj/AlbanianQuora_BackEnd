using Moq;
using Microsoft.EntityFrameworkCore;
using AlbanianQuora.Entities;
using AlbanianQuora.Controllers;
using AlbanianQuora.Data;
using Microsoft.AspNetCore.Mvc;

namespace AlbanianQuora.tests
{
    public class UnitTest1
    {
        private readonly Mock<DbSet<User>> _mockSet;
        private readonly Mock<UserDbContext> _mockContext;
        private readonly UserController _controller;

        public UnitTest1()
        {
            _mockSet = new Mock<DbSet<User>>();
            _mockContext = new Mock<UserDbContext>();
            _controller = new UserController(_mockContext.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = Guid.NewGuid(), FirstName = "Test", LastName = "User", Email = "test@example.com", Password = "password" },
                new User { UserId = Guid.NewGuid(), FirstName = "Test2", LastName = "User2", Email = "test2@example.com", Password = "password2" }
            }.AsQueryable();

            _mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(users.Count(), returnValue.Count);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFoundResult_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockSet.Setup(m => m.FindAsync(userId)).ReturnsAsync((User)null);
            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}