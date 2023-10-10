using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibarary.Dtos;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(ResponseDto<T> responseDto) where T  : class 
        {
            return new ObjectResult(responseDto)
            {
                StatusCode = responseDto.Status,
            };
        }
    }
}
