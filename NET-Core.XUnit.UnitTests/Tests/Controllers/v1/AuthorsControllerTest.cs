using Moq;
using Xunit;
using AutoMapper;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using ASP.NET.Core.WebAPI.Models.DTOs;
using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Controllers.v1;
using NET.Core.Library.Domain.Infrastructure.Contracts;

namespace NET.Core.XUnit.UnitTests.Tests.Controllers.v1
{
    public class AuthorsControllerTest
    {
        [Fact]
        public void GetAuthorById_EnsureReturnsSpecificAuthorWhenValidIdIsPassed()
        {
            // Arrange
            int id = 1;
            Author author = new() { Id = id };

            List<Author> authorsMock = new() { author };
            Mock<IQueryable<Author>> authorQueryableMock = authorsMock.AsQueryable().BuildMock();

            Mock<IRepository<Author>> authorRepositoryMock = new();
            authorRepositoryMock.Setup(repository => repository.AsQueryable()).Returns(authorQueryableMock.Object).Verifiable();

            Mock<IMapper> mapperMock = new();
            mapperMock.Setup(mapper => mapper.Map<Author, AuthorDTO>(It.IsAny<Author>())).Returns(new AuthorDTO() { Id = id, FirstName = "Test" });

            Mock<ILogger<AuthorsController>> loggerMock = new();
            Mock<IUrlHelper> urlHelperMock = new();

            // Act
            AuthorsController authorsController = new(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
            IActionResult actionResult = authorsController.GetAuthorById(id)?.Result;

            // Assert
            if (actionResult != null && actionResult is OkObjectResult)
            {
                AuthorDTO authorDTO_Received = ((actionResult as OkObjectResult).Value) as AuthorDTO;
                Assert.Equal(authorDTO_Received?.Id, id);
            }
        }

        [Fact]
        public void GetAuthorById_EnsureReturnsNotFoundWhenInvalidIdIsPassed()
        {
            // Arrange
            int id = 1;
            int invalidId = 104043;
            Author author = new() { Id = id };

            List<Author> authorsMock = new() { author };
            Mock<IQueryable<Author>> authorQueryableMock = authorsMock.AsQueryable().BuildMock();

            Mock<IRepository<Author>> authorRepositoryMock = new();
            authorRepositoryMock.Setup(repository => repository.AsQueryable()).Returns(authorQueryableMock.Object).Verifiable();

            Mock<IMapper> mapperMock = new();
            mapperMock.Setup(mapper => mapper.Map<Author, AuthorDTO>(It.IsAny<Author>())).Returns(new AuthorDTO() { Id = id, FirstName = "Test" });

            Mock<ILogger<AuthorsController>> loggerMock = new();
            Mock<IUrlHelper> urlHelperMock = new();

            // Act
            AuthorsController authorsController = new(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
            IActionResult actionResult = authorsController.GetAuthorById(invalidId)?.Result;

            // Assert
            if (actionResult != null && (actionResult is ObjectResult) && ((actionResult as ObjectResult).Value is ProblemDetails))
            {
                Assert.True(((actionResult as ObjectResult).Value as ProblemDetails).Status == StatusCodes.Status404NotFound);
            }
        }

        [Fact]
        public void GetAuthorById_EnsureReturnsInternalServerErrordWhenEncountersError()
        {
            // Arrange
            int id = 1;

            IQueryable<Author> expectingNull = null;

            Mock<IRepository<Author>> authorRepositoryMock = new();
            authorRepositoryMock.Setup(repository => repository.AsQueryable()).Returns(expectingNull);

            Mock<IMapper> mapperMock = new();
            Mock<ILogger<AuthorsController>> loggerMock = new();
            Mock<IUrlHelper> urlHelperMock = new();

            // Act
            AuthorsController authorsController = new(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
            IActionResult actionResult = authorsController.GetAuthorById(id)?.Result;

            // Assert
            if (actionResult != null && (actionResult is ObjectResult) && ((actionResult as ObjectResult).Value is ProblemDetails))
            {
                Assert.True(((actionResult as ObjectResult).Value as ProblemDetails).Status == StatusCodes.Status500InternalServerError);
            }
        }
    }
}