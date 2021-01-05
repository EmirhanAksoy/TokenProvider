using ArcelikAuthProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRoleById(string Id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == Id).FirstOrDefaultAsync();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            if (string.IsNullOrEmpty(role.Id))
                role.Id = Guid.NewGuid().ToString();

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                return BadRequest(result.Errors.ToList());

            var createdRole = await _roleManager.FindByNameAsync(role.Name);
            return Ok(createdRole);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {

            var role = await _roleManager.Roles.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (role == null)
                return NotFound("Role not found");

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return Ok(result.Errors.ToList());

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Role role)
        {
            if (string.IsNullOrEmpty(role.Id))
                return BadRequest();

            IdentityResult result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return Ok(result.Errors.ToList());


            return Ok();
        }
    }
}