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
            var result = await _controller.GetUsers();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFoundResult_WhenUserDoesNotExist()
        {
            var result = await _controller.GetUser(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

    }
}