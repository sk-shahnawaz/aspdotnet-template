using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Swashbuckle.AspNetCore.Annotations;

using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Models.DTOs;
using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;
using ASP.NET.Core.WebAPI.Models.DTOs.Contracts;

namespace ASP.NET.Core.WebAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces(AppResources.MimeTypeApplicationJson)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors(AppResources.CorsPolicyName)]
    [SwaggerTag(AppResources.ApiDescriptor + nameof(Author))]
    public class AuthorsController : ControllerBase, IDisposable
    {
        #region --- Global Variables ---

        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IUrlHelper _urlHelper;
        private readonly IRepository<Author> _authorsRepository;

        #endregion --- Global Variables ---

        #region --- Constructors ---

        public AuthorsController(ILogger<AuthorsController> logger, IMapper mapper, IUrlHelper urlHelper, IRepository<Author> authorsRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _authorsRepository = authorsRepository;
        }

        #endregion --- Constructors ---

        #region --- GET Single Author By ID ---

        /// <summary>
        /// Gets a single author by author ID.
        /// </summary>
        /// <param name="id">ID of the author</param>
        /// <returns>A single author</returns>
        [HttpGet]
        [Microsoft.AspNet.OData.EnableQuery]
        [Route("{id:long:min(1)}", Name = nameof(GetAuthorById))]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Returns a specific Author")]
        public async Task<IActionResult> GetAuthorById(long id)
        {
            try
            {
                Author author = await _authorsRepository.GetAsync(author => author.Id == id, trackEntity: false, cancellationToken: default, includes: author => author.Books);
                if (author != null)
                {
                    AuthorDTO authorDTO = _mapper.Map<Author, AuthorDTO>(author);
                    authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, id, ControllerContext);
                    return Ok(authorDTO);
                }
                else
                {
                    return Problem(string.Format(AppResources.NoItemFoundMessage, nameof(Author), nameof(Author.Id), id), statusCode: StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion --- GET Single Author By ID ---

        #region --- GET All Authors ---

        /// <summary>
        /// Gets all authors with pagination.
        /// </summary>
        /// <param name="pagination">Pagination request (PageNumber, PageSize)</param>
        /// <returns>List of authors</returns>
        [HttpGet]
        [Microsoft.AspNet.OData.EnableQuery]
        [Route("", Name = nameof(GetAuthors))]
        [ProducesResponseType(typeof(PaginationResponse<AuthorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Returns all Authors")]
        public async Task<IActionResult> GetAuthors([FromQuery] PaginationRequest pagination)
        {
            try
            {
                IQueryable<Author> query = _authorsRepository.AsQueryable(trackEntity: false).Include(author => author.Books);
                List<Author> authors = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
                if (authors?.Count > 0)
                {
                    List<AuthorDTO> authorDTOs = _mapper.Map<List<Author>, List<AuthorDTO>>(authors);
                    authorDTOs.ForEach(authorDTO => authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, authorDTO.Id, ControllerContext));
                    int totalItemsCount = await _authorsRepository.AsQueryable().CountAsync();

                    var paginationResponse = new PaginationResponse<List<AuthorDTO>>(pagination.PageNumber, pagination.PageSize, totalItemsCount, authorDTOs);
                    return Ok(paginationResponse);
                }
                else
                {
                    return Problem(string.Format(AppResources.NoItemsFoundMessage, string.Concat(nameof(Author), "s")), statusCode: StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion --- GET All Authors ---

        #region --- POST / Create Single Author ---

        /// <summary>
        /// Creates an author in database.
        /// </summary>
        /// <param name="authorDTO">Author details</param>
        /// <param name="unitOfWork">Abstraction representing a unit of work in database</param>
        /// <returns>Newly created author</returns>
        [HttpPost]
        [Route("create", Name = nameof(CreateAuthor))]
        [Consumes(AppResources.MimeTypeApplicationJson)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Creates an Author")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDTO authorDTO, [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                if (!await _authorsRepository.AsQueryable(trackEntity: false).AnyAsync(storedAuthor => string.Equals(storedAuthor.Email, authorDTO.EmailAddress)))
                {
                    Author author = _mapper.Map<AuthorDTO, Author>(authorDTO);
                    if (author != null)
                    {
                        await _authorsRepository.AddAsync(author);
                        if (await unitOfWork.SaveChangesAsync() > 0)
                        {
                            authorDTO.Id = author.Id;
                            authorDTO.Books = author.Books?.Count > 0 ? _mapper.Map<List<Book>, List<BookDTO>>(author.Books) : null;
                            authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, authorDTO.Id, ControllerContext);
                            return Ok(authorDTO);
                        }
                        else
                        {
                            return Problem(statusCode: StatusCodes.Status422UnprocessableEntity);
                        }
                    }
                    else
                    {
                        return Problem(string.Format(AppResources.BadRequestDueToAutoMapFailure), statusCode: StatusCodes.Status400BadRequest);
                    }
                }
                else
                {
                    return Problem(string.Format(AppResources.ForbidRequestDueToUniqueConstraintFailure, nameof(authorDTO.EmailAddress), authorDTO.EmailAddress), statusCode: StatusCodes.Status422UnprocessableEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
            finally
            {
                if (unitOfWork != null)
                    unitOfWork.Dispose();
            }
        }

        #endregion --- POST / Create Single Author ---

        #region --- PUT / Replace Single Author ---

        /// <summary>
        /// Replaces an author details in database.
        /// </summary>
        /// <param name="id">Unique ID of the author</param>
        /// <param name="authorDTO">Author details</param>
        /// <param name="bookRepository">Abstraction representing books data from database</param>
        /// <param name="unitOfWork">Abstraction representing unit of work in database</param>
        /// <returns>Replaced author</returns>
        [HttpPut]
        [Route("{id:long:min(1)}/replace", Name = nameof(ReplaceAuthor))]
        [Consumes(AppResources.MimeTypeApplicationJson)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Replaces a specific Author")]
        public async Task<IActionResult> ReplaceAuthor(long id, [FromBody] AuthorDTO authorDTO, [FromServices] IRepository<Book> bookRepository, [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                Author author;
                if ((author = await _authorsRepository.AsQueryable().Include(author => author.Books).FirstOrDefaultAsync(storedAuthor => storedAuthor.Id == id)) != null)
                {
                    if (author.Books?.Count > 0)
                    {
                        author.Books.ForEach(storedBook =>
                        {
                            bookRepository.Delete(storedBook);
                        });
                    }

                    author.FirstName = authorDTO.FirstName;
                    author.MiddleName = authorDTO.MiddleName;
                    author.LastName = authorDTO.LastName;
                    author.Address = authorDTO.Address;
                    author.PhoneNumber = authorDTO.PhoneNumber;
                    author.Books = authorDTO.Books?.Count > 0 ? _mapper.Map<List<BookDTO>, List<Book>>(authorDTO.Books) : null;

                    if (author != null)
                    {
                        _authorsRepository.Update(author);
                        await unitOfWork.SaveChangesAsync();

                        authorDTO = _mapper.Map<Author, AuthorDTO>(author);
                        authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, id, ControllerContext);
                        return Ok(authorDTO);
                    }
                    else
                    {
                        return Problem(string.Format(AppResources.BadRequestDueToAutoMapFailure), statusCode: StatusCodes.Status400BadRequest);
                    }
                }
                else
                {
                    return Problem(string.Format(AppResources.NoItemFoundMessage, nameof(Author), nameof(Author.Id), id), statusCode: StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
            finally
            {
                if (unitOfWork != null)
                    unitOfWork.Dispose();
            }
        }

        #endregion --- POST / Create Single Author ---

        #region --- PATCH / Update Single Author ---

        /// <summary>
        /// Updates an author in database.
        /// </summary>
        /// <param name="id">Unique ID of the author</param>
        /// <param name="unitOfWork">Abstraction representing unit of work in database</param>
        /// <param name="jsonPatchDocument">JSON patch document</param>
        /// <returns>Updated author</returns>
        [HttpPatch("{id:long:min(1)}/update", Name = nameof(UpdateAuthor))]
        [Consumes(AppResources.MimeTypeApplicationJsonPatchJson)]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [SwaggerOperation(Summary = "Updates a specific Author")]
        public async Task<IActionResult> UpdateAuthor(long id, [FromServices] IUnitOfWork unitOfWork, [FromBody] JsonPatchDocument<Author> jsonPatchDocument)
        {
            try
            {
                if (jsonPatchDocument?.Operations?.Count > 0)
                {
                    Author author;
                    if ((author = await _authorsRepository.AsQueryable().FirstOrDefaultAsync(storedAuthor => storedAuthor.Id == id)) != null)
                    {
                        jsonPatchDocument.ApplyTo(author);
                        if (ModelState.IsValid)
                        {
                            if (await unitOfWork.SaveChangesAsync() > 0)
                            {
                                AuthorDTO authorDTO = _mapper.Map<Author, AuthorDTO>(author);
                                authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, id, ControllerContext);
                                return Ok(authorDTO);
                            }
                            else
                            {
                                return Problem(statusCode: StatusCodes.Status422UnprocessableEntity);
                            }
                        }
                        else
                        {
                            ValidationProblemDetails validationProblemDetails = new(ModelState)
                            {
                                Status = StatusCodes.Status400BadRequest,
                                Instance = ControllerContext.HttpContext.TraceIdentifier,
                                Title = AppResources.ModelValidationErrorMessage,
                                Detail = "Bad Request"
                            };
                            return new BadRequestObjectResult(validationProblemDetails);
                        }
                    }
                    else
                    {
                        return Problem(string.Format(AppResources.NoItemFoundMessage, nameof(Author), nameof(Author.Id), id), statusCode: StatusCodes.Status404NotFound);
                    }
                }
                else
                {
                    return Problem(statusCode: StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion --- PATCH / Update Single Author ---

        #region --- DELETE Single Author ---

        /// <summary>
        /// Deletes author details from database.
        /// </summary>
        /// <param name="id">Unique ID of the author</param>
        /// <param name="unitOfWork">Abstraction representing unit of work in database</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:long:min(1)}/delete", Name = nameof(DeleteAuthor))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Deletes a specific Author")]
        public async Task<IActionResult> DeleteAuthor(long id, [FromServices] IUnitOfWork unitOfWork)
        {
            try
            {
                Author author;
                if ((author = await _authorsRepository.AsQueryable().FirstOrDefaultAsync(storedAuthor => string.Equals(storedAuthor.Id, id))) != null)
                {
                    _authorsRepository.Delete(author);
                    if (await unitOfWork.SaveChangesAsync() > 0)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Problem(AppResources.UnprocessableEntity, statusCode: StatusCodes.Status422UnprocessableEntity);
                    }
                }
                else
                {
                    return Problem(string.Format(AppResources.NoItemFoundMessage, nameof(Author), nameof(Author.Id), id), statusCode: StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion --- DELETE Single Author ---

        public void Dispose()
        {
            if (_authorsRepository != null)
                _authorsRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}