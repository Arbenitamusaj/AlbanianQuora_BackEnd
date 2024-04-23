using Microsoft.EntityFrameworkCore;
using AlbanianQuora.Entities;
using AlbanianQuora.Controllers;
using AlbanianQuora.Data;
using Microsoft.AspNetCore.Mvc;

namespace AlbanianQuora.tests
{
    public class UnitTest1
    {

        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql("Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=postgres")
            .Options;
            //Testing
            var dbContext = new UserDbContext(options);
            var controller = new UserController(dbContext);

            //// Act
            var user1 = new User { UserId = Guid.NewGuid(), FirstName = "Test" , LastName = "Test2", Email = "test@test.com", Password = "password", CreatedAt = new DateTime() };
            var user2 = new User { UserId = Guid.NewGuid(), FirstName = "Test12", LastName = "Test3", Email = "test1@test.com", Password = "password2", CreatedAt = new DateTime() };
            
            dbContext.Users.Add(user1);
            dbContext.Users.Add(user2);
            await dbContext.SaveChangesAsync();

            var result = await controller.GetUsers() as OkObjectResult;

            //// Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetUser_ReturnsAddedItem()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql("Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=postgres")
            .Options;

            var dbContext = new UserDbContext(options);
            var controller = new UserController(dbContext);
            await dbContext.SaveChangesAsync();

            // Act
            var user = new User { UserId = Guid.NewGuid(), FirstName = "Test", LastName = "Test2", Email = "test@test.com", Password = "password", CreatedAt = new DateTime() };
            var result = await controller.GetUser(user.UserId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

    }
}