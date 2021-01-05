using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArcelikAuthProvider.Models;
using ArcelikAuthProvider.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArcelikAuthProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
      
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _userManager.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            User user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.Id = Guid.NewGuid().ToString();
           
            IdentityResult result = await _userManager.CreateAsync(user, user.Password);

            var newUser = await _userManager.FindByEmailAsync(user.Email);

            if (!result.Succeeded)
                return BadRequest(result.Errors.ToList());

            //Clear roles
            var roles = await this._userManager.GetRolesAsync(user);
            if (roles.Count > 0)
                await this._userManager.RemoveFromRolesAsync(newUser, roles.ToArray());

            if (user.RoleNames.Count > 0)
            {
                foreach (var rolName in user.RoleNames)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, rolName);
                }
            }
           
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUserById(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return BadRequest();

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return Ok(result.Errors.ToList());

            return Ok();
        }

        [HttpGet("{Id}/Roles")]
        public async Task<IActionResult> GetUserRoles(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound("User not found");

            var roles = _userManager.GetRolesAsync(user);

            return Ok(roles.Result);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            User _user = await _userManager.FindByIdAsync(changePasswordViewModel.Id);

            if (_user == null)
                return NotFound();

            if (_user.Password != changePasswordViewModel.OldPassword)
                return NotFound();

            _user.PasswordHash = _userManager.PasswordHasher.HashPassword(_user, changePasswordViewModel.NewPassword);
           

            await _userManager.UpdateAsync(_user);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            User _user = await _userManager.FindByIdAsync(user.Id);

            if (_user == null)
                return NotFound();

            //Update user data.
            _user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.Password);
            _user.UserName = user.UserName;
            _user.IsActive = user.IsActive;
            _user.Password = user.Password;
            _user.Email = user.Email;
            _user.NormalizedEmail = user.Email.ToUpper();
            _user.NormalizedUserName = user.UserName.ToUpper();
           
            IdentityResult updateResult = await _userManager.UpdateAsync(_user);

            var updatedUser = await _userManager.FindByIdAsync(user.Id);

            var roles = await this._userManager.GetRolesAsync(updatedUser);
            if (roles.Count > 0)
                await this._userManager.RemoveFromRolesAsync(updatedUser, roles.ToArray());

            if (user.RoleNames.Count > 0)
            {
                foreach (var rolName in user.RoleNames)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(updatedUser, rolName);
                }
            }

            return Ok();
        }

        [HttpPost("Role")]
        public async Task<IActionResult> SetUserRoles(UserRoleViewModel userRoleViewModel)
        {
            if (userRoleViewModel.Roles.Count == 0 || string.IsNullOrEmpty(userRoleViewModel.UserId))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);

            //remove roles of user
            var roles = await this._userManager.GetRolesAsync(user);
            await this._userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (user == null)
                return NotFound();

            foreach (var role in userRoleViewModel.Roles)
            {
                IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            }

            return Ok();
        }
    }

}