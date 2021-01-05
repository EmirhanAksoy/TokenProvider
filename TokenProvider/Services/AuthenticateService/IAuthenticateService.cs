using ArcelikAuthProvider.Models;
using ArcelikAuthProvider.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Services.AuthenticateService
{
    public interface IAuthenticateService
    {
        public Task<TokenViewModel> GetToken(User user);
        public Task<TokenViewModel> RefreshToken(TokenViewModel tokenViewModel);
        public Task<(bool success, string message)> SignIn(User user, string password);
        public Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        public Task SignOut();
    }
}
