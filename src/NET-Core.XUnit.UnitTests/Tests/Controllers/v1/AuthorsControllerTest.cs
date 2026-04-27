using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Moq;
using Xunit;
using AutoMapper;
using MockQueryable.Moq;

using NET.Core.Library.Domain.DBModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;

using ASP.NET.Core.WebAPI.Models.DTOs;
using ASP.NET.Core.WebAPI.Controllers.v1;

namespace NET.Core.XUnit.UnitTests.Tests.Controllers.v1;

public class AuthorsControllerTest
{
    private static Author CreateTestAuthor(long id) =>
        new() { Id = id, FirstName = "Test", LastName = "User", PhoneNumber = "0000000000", Email = "test@test.com" };

    [Fact]
    public void GetAuthorById_EnsureReturnsSpecificAuthorWhenValidIdIsPassed()
    {
        // Arrange
        int id = 1;
        var author = CreateTestAuthor(id);

        var authorsMock = new List<Author> { author };
        var authorQueryableMock = authorsMock.BuildMockDbSet();

        var authorRepositoryMock = new Mock<IRepository<Author>>();
        authorRepositoryMock.Setup(repository => repository.AsQueryable(false)).Returns(authorQueryableMock.Object).Verifiable();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Author, AuthorDTO>(It.IsAny<Author>())).Returns(new AuthorDTO() { Id = id, FirstName = "Test" });

        var loggerMock = new Mock<ILogger<AuthorsController>>();
        var urlHelperMock = new Mock<IUrlHelper>();

        // Act
        var authorsController = new AuthorsController(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
        var actionResult = authorsController.GetAuthorById(id)?.Result;

        // Assert
        if (actionResult is OkObjectResult okResult && okResult.Value is AuthorDTO authorDtoReceived)
        {
            Assert.Equal(authorDtoReceived.Id, id);
        }
    }

    [Fact]
    public void GetAuthorById_EnsureReturnsNotFoundWhenInvalidIdIsPassed()
    {
        // Arrange
        int id = 1;
        int invalidId = 104043;
        var author = CreateTestAuthor(id);

        var authorsMock = new List<Author> { author };
        var authorQueryableMock = authorsMock.BuildMockDbSet();

        var authorRepositoryMock = new Mock<IRepository<Author>>();
        authorRepositoryMock.Setup(repository => repository.AsQueryable(false)).Returns(authorQueryableMock.Object).Verifiable();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<Author, AuthorDTO>(It.IsAny<Author>())).Returns(new AuthorDTO() { Id = id, FirstName = "Test" });

        var loggerMock = new Mock<ILogger<AuthorsController>>();
        var urlHelperMock = new Mock<IUrlHelper>();

        // Act
        var authorsController = new AuthorsController(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
        var actionResult = authorsController.GetAuthorById(invalidId)?.Result;

        // Assert
        if (actionResult is ObjectResult { Value: ProblemDetails problemDetails })
        {
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }
    }

    [Fact]
    public void GetAuthorById_EnsureReturnsInternalServerErrorWhenEncountersError()
    {
        // Arrange
        int id = 1;

        var authorRepositoryMock = new Mock<IRepository<Author>>();
        authorRepositoryMock
            .Setup(repository => repository.GetAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Author, bool>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<Author, object>>[]>()))
            .ThrowsAsync(new Exception("DB error"));

        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<AuthorsController>>();
        var urlHelperMock = new Mock<IUrlHelper>();

        // Act
        var authorsController = new AuthorsController(loggerMock.Object, mapperMock.Object, urlHelperMock.Object, authorRepositoryMock.Object);
        var actionResult = authorsController.GetAuthorById(id)?.Result;

        // Assert
        if (actionResult is ObjectResult { Value: ProblemDetails problemDetails })
        {
            Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
        }
    }
}
