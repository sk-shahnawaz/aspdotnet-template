using System;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Models.DTOs;
using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;
using ASP.NET.Core.WebAPI.Models.DTOs.Contracts;

namespace ASP.NET.Core.WebAPI.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
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

        #region --- GET Single Author By Email Address ---

        /// <summary>
        /// Gets a single author by author's email address.
        /// </summary>
        /// <param name="email">Email address of the author</param>
        /// <returns>A single author details</returns>
        [HttpGet]
        [Microsoft.AspNet.OData.EnableQuery]
        [Route("{email:email}", Name = nameof(GetAuthorByEmailAddress))]
        [ProducesResponseType(typeof(AuthorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Returns a specific Author by Email Address")]
        public async Task<IActionResult> GetAuthorByEmailAddress(string email)
        {
            try
            {
                Author author = await _authorsRepository.AsQueryable().Include(author => author.Books).FirstOrDefaultAsync(author => string.Equals(author.Email, email));
                if (author != null)
                {
                    AuthorDTO authorDTO = _mapper.Map<Author, AuthorDTO>(author);
                    authorDTO.Links = LinkDTO.GenerateHateoasLinks(_urlHelper, authorDTO.Id, ControllerContext);
                    return Ok(authorDTO);
                }
                else
                {
                    return Problem(string.Format(AppResources.NoItemFoundMessage, nameof(Author), nameof(Author.Email), email), statusCode: StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(AppResources.ActionExceptionMessage, new { ex });
                return Problem(AppResources.InternalServerErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion --- GET Single Author By Email Address ---

        public void Dispose()
        {
            if (_authorsRepository != null)
                _authorsRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}