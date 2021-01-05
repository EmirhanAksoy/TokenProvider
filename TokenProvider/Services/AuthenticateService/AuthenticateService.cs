using ArcelikAuthProvider.Context;
using ArcelikAuthProvider.Helper;
using ArcelikAuthProvider.Models;
using ArcelikAuthProvider.Services.RefreshTokenService;
using ArcelikAuthProvider.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Services.AuthenticateService
{
    public class AuthenticateService : IAuthenticateService
    {
        protected readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public AuthenticateService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            ApplicationDbContext applicationDbContext,
            IRefreshTokenService refreshTokenService)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _applicationDbContext = applicationDbContext;
        }

        public virtual async Task<(bool success, string message)> SignIn(User user, string password)
        {
            SignInResult singInResult = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            
            return (singInResult.Succeeded, !singInResult.Succeeded ? "" : "Kullanıcı adı veya şifre doğru değil");
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            User user = usernameOrEmail.Contains("@") == true ?
                        await _userManager.FindByEmailAsync(usernameOrEmail) :
                        await _userManager.FindByNameAsync(usernameOrEmail);

            return user;
        }

        public async Task<TokenViewModel> GetToken(User user)
        {
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            string jwtToken = TokenHelper.GenerateJwtToken(user, userRoles, _configuration);

            var oldRefreshToken = await _refreshTokenService.GetRefreshTokenByUserId(user.Id);

            string refreshToken;
            if (oldRefreshToken == null)
            {
                refreshToken = TokenHelper.GenerateRefreshToken();
                await _refreshTokenService.AddAsync(refreshToken, user.Id);
            }
            else
                refreshToken = oldRefreshToken.Token;

            return new TokenViewModel()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokenViewModel> RefreshToken(TokenViewModel tokenViewModel)
        {
            string userId = TokenHelper.GetUserIdFromExpiredToken(tokenViewModel.JwtToken);

            User user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            var refreshToken = await _refreshTokenService.GetRefreshToken(user.Id, tokenViewModel.RefreshToken);

            if (refreshToken == null)
                return null;

          

            string jwtToken = TokenHelper.GenerateJwtToken(user, userRoles, _configuration);

           
            return new TokenViewModel()
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
            };
        }

    }
}
