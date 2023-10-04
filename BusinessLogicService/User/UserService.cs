using BusinessLogicService.User.UserTemplate;
using Contracts;
using Contracts.Dto.Enums;
using Contracts.Dto.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.User
{
    public class UserService : IUserService
    {
        private readonly IConfiguration config;
        public UserService(IConfiguration config)
        {
            this.config = config;
        }

        private async Task<string> GenerateTokenAsync(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.UserPermission.ToString()),
                new Claim("ModulePermissions", string.Join(';', user.ModulePermissions)),
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credKey);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateToken(UserDto user) => GenerateTokenAsync(user).Result;

        public async Task<Result<string>> LoginAsync(string user, string pass)
        {
            User.UserTemplate.UserTemplate ut = null;
            if(user == "admin" && pass == "admin")
                ut = new AdminUser();
            else if(user == "user" && pass == "user")
                ut = new BaseUser();
            else
                return new Result<string>() { ResultCode = (int)HttpStatusCode.BadRequest, Message = "User not found"};

            var res = await GenerateTokenAsync(ut.UserDto);
            return new Result<string>() { Data = res };
        }
        public Result<string> Login(string user, string pass) => LoginAsync(user, pass).Result;
    }
}
