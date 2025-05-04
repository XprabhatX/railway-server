using Authentication.Models;
using Authentication.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        readonly UserManager<User> _userManager;
        readonly IConfiguration _configuration;

        public AccountsController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp SignUpObj)
        {
            try {
                var user = new User
                {
                    UserName = SignUpObj.UserName,
                    Email = SignUpObj.Email,
                    AadharNumber = SignUpObj.AadharNumber
                };

                if (SignUpObj.Role != "User" && SignUpObj.Role != "Admin")
                    return BadRequest("Please specify correct role.");

                var aadharRegex = new Regex(@"^\d{12}$");
                if (!aadharRegex.IsMatch(user.AadharNumber))
                    return BadRequest("Invalid Aadhar number. It must be exactly 12 digits.");

                var result = await _userManager.CreateAsync(user, SignUpObj.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SignUpObj.Role);
                    return Ok("User account created.");
                }
                return BadRequest("Failed to create account.");
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Caught error: {ex.Message}");
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignIn SignInObj)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(SignInObj.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, SignInObj.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    Console.WriteLine($"Aadhar {user.AadharNumber} logged in with Role: {string.Join(",", roles)}");

                    var token = await GenerateJwtToken(user);
                    return Ok(token);
                }
                return BadRequest("Incorrect Email/Password");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Caught error: {ex.Message}");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users
                    .Select(u => new UserListingDto 
                    {
                        Username = u.UserName,
                        Email = u.Email,
                        AadharNumber = u.AadharNumber
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"Caught error: {ex.Message}");
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized("Email claim missing in token.");

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return NotFound("User not found.");
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new {
                user.UserName,
                user.Email,
                user.AadharNumber,
                roles
            });
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
