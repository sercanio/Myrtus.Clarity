using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Myrtus.Clarity.Core.WebApi;

namespace Myrtus.CMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ISender _sender;
        protected readonly IErrorHandlingService _errorHandlingService;

        public BaseController(ISender sender, IErrorHandlingService errorHandlingService)
        {
            _sender = sender;
            _errorHandlingService = errorHandlingService;
        }
    }
}
