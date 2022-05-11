using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonManagement.Api.Models;
using System;
using System.Threading.Tasks;

namespace PersonManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var user = new IdentityUser { UserName = registerModel.UserName, Email = registerModel.Email };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            return Ok(new Response(true));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Login);

            if (user == null)
            {
                return new BadRequestObjectResult(new Response(false, "User doesn't exist."));
            }

            var signInResult = await _signInManager.PasswordSignInAsync(loginModel.Login, loginModel.Password, false, false);

            if (!signInResult.Succeeded)
            {
                return new BadRequestObjectResult(new Response(false, "Password is incorrect."));
            }

            return Ok(new Response(true));
        }
    }
}
