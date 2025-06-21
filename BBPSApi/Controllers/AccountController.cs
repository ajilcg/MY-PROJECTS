using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BBPSApi.Model;
using BBPSApi.Service;

namespace BBPSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private JwtService _jwtService;

        public AccountController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
        {
            var data = new LoginRequestModel();
            data.Username = request.Username;
            data.Password =request.Password;

            var result = await _jwtService.Authenticate(data);
            if (result is null)
                 return Unauthorized();
 
            return Ok(result);
   
        }
    }
}
