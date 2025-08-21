using ECommerce.API.DTOs;
using RPA.Core.Models;
using RPA.Infrastracture.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Infrastracture.Services.Abstraction
{
    public interface IAuthService
    {
        public Task<bool> DisableUser(string userId);
        public Task<AppUser> GetUser(string userId); 
        public Task<IEnumerable<GetEmployeesDTO>> GetAllUSers(string email="");
        public Task<bool> IsAuthenticated(string email);
        public Task<string?> GenerateJwtTokenAsync(AppUser user);
        public Task<string> login(LoginDTO loginDto);
        public Task<bool> logout();
        public Task<string> register(RegisterDTO registerDTO, string role);


    }
}
