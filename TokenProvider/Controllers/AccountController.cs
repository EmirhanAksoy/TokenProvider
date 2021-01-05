using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArcelikAuthProvider.Services.AuthenticateService;
using ArcelikAuthProvider.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ArcelikAuthProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
       
        public AccountController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SingIn(SignInViewModel signInViewModel)
        {
            var user = await _authenticateService.GetUserByUsernameOrEmail(signInViewModel.Username);

            if (user == null)
                return BadRequest(new { message = "Kullanıcı adı veya şifre doğru değil" });

            var result = await _authenticateService.SignIn(user, signInViewModel.Password);

            if (!result.success)
                return BadRequest(new { message = result.message });


            TokenViewModel tokenViewModel = await _authenticateService.GetToken(user);

            return Ok(tokenViewModel);
        }

        [HttpPost("NewToken")]
        public async Task<IActionResult> NewToken(TokenViewModel tokenViewModel)
        {
            var result = await _authenticateService.RefreshToken(tokenViewModel);

            if (result == null)
                return Unauthorized("Invalid token");

            return Ok(result);
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await _authenticateService.SignOut();
            return Ok();
        }

    }
}