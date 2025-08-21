using ECommerce.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPA.Infrastracture.DTOs;
using RPA.Infrastracture.Services.Abstraction;


namespace RPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string email="")
        {
            var users = await _authService.GetAllUSers(email);
            if (users == null || !users.Any()) {
                return BadRequest(new { success = false, message = "No Users Found" } );
            }
            return Ok(new { success=true,users});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _authService.GetUser(id);
            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }
            return Ok(new { success = true, user });

        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var jwt = await _authService.register(registerDto, registerDto.role); 
            if(jwt == null)
            {
                return BadRequest(new { success=false,message="Faild to create user"});
            }
            return Ok(new {success=true,jwt});

        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginDTO loginDto)
        {
            var jwt = await _authService.login(loginDto); 
            if(jwt == null)
            {
                return BadRequest(new { success = false, message = "email or password is wrong" }); 
            }
            return Ok(new { success = true, jwt});  
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DisbaleEmployeeDTO dto)
        {
            var result = await _authService.DisableUser(dto.Id);
            if (result)
            {
                return Ok(new { success = true, message = "User Disabled Successfully" });
            }
            return Ok(new { success = false, message = "Faild to disable user" });

        }


    }
}
