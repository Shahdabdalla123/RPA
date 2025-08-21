using ECommerce.API.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RPA.Core.Models;
using RPA.Infrastracture.Context;
using RPA.Infrastracture.DTOs;
using RPA.Infrastracture.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Infrastracture.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; 

        public AuthService(UserManager<AppUser> userManager
            , IConfiguration configuration
            , RoleManager<IdentityRole> roleManager
            ,ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context; 
            //_roleManager = roleManager;
        }

        //JWT
        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(3),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> register(RegisterDTO registerDTO, string role)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await IsAuthenticated(registerDTO.Email))
            {
                return "existed";
            }

            var user = new AppUser
            {
                FirstName = registerDTO.FName,
                LastName = registerDTO.LName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
            };
            await _userManager.CreateAsync(user, registerDTO.Password);
            if (!string.IsNullOrEmpty(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return await GenerateJwtTokenAsync(user);
        }


        public async Task<string> login(LoginDTO LoginUser)
        {

            if (await IsAuthenticated(LoginUser.Email))
            {
                var user = await _userManager.FindByEmailAsync(LoginUser.Email);
                if (await _userManager.CheckPasswordAsync(user, LoginUser.Password))
                {
                    return await GenerateJwtTokenAsync(user);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Task<bool> logout()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsAuthenticated(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return !(user == null);
        }

        public async Task<IEnumerable<GetEmployeesDTO>> GetAllUSers(string email = "")
        {
            var users = await _userManager.Users.Where(u => u.Email.Contains(email)).Select(u => new GetEmployeesDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                isActive = u.isActive,
            }).ToListAsync();
            return users; 
        }

        public async Task<bool> DisableUser(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.Id==userId);
            if (user == null) return false; 
            
            user.isActive = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded; 
        }

        public Task<AppUser?> GetUser(string userId)
        {
            return _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
