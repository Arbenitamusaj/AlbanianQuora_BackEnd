using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbanianQuora.Controllers;
using AlbanianQuora.Data;
using AlbanianQuora.Entities;
using AlbanianQuora.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Runtime.Intrinsics.X86;

namespace AlbanianQuora.tests
{
    public class QuestionCategoryControllerTests
    {
        [Fact]
        public async Task GetQuestionCategories_ReturnsOkResult()
        {

            var options = new DbContextOptionsBuilder<UserDbContext>()
             .UseNpgsql("Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=postgres").Options;

            var dbContext = new UserDbContext(options);
            var controller = new QuestionCategoryController(dbContext);


            // Arrange

            var QuestionCategory1 = new QuestionCategory { Id = Guid.NewGuid(), Category = "Shoqeria" };
            var QuestionCategory2 = new QuestionCategory { Id = Guid.NewGuid(), Category = "Ekonomike" };


            dbContext.QuestionCategories.Add(QuestionCategory1);
            dbContext.QuestionCategories.Add(QuestionCategory2);
            await dbContext.SaveChangesAsync();

            var result = await controller.GetQuestionCategories() as OkObjectResult;

            //// Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetUser_ReturnsAddedItem()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserDbContext>()
           .UseNpgsql("Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=postgres").Options;

            var dbContext = new UserDbContext(options);
            var controller = new QuestionCategoryController(dbContext);
            await dbContext.SaveChangesAsync();

            // Act
            var questionCategory = new QuestionCategory { Id = Guid.NewGuid(), Category = "Shoqeria"};
            var questionCategory2 = new QuestionCategory { Id = Guid.NewGuid(), Category = "Lajme" };
            dbContext.QuestionCategories.Add(questionCategory);
            dbContext.QuestionCategories.Add(questionCategory2);
            var result = await controller.GetQuestionCategory(questionCategory.Id) as OkObjectResult;
            var result2 = await controller.GetQuestionCategory(questionCategory2.Id) as OkObjectResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

    }
}


//.UseNpgsql("Host=localhost; Port=5432; Database=mydb; Username=postgres; Password=postgres").Options;