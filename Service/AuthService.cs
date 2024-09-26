using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vueapi.Dtos.UserDto;
using Vueapi.Model;

namespace Vueapi.Service
{
    public class AuthService
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _configuration = configuration;
        }


        public async Task<ActionResult<Object>> Register(UserDto user)
        {
            var password = user.Password;
            var User = new User()
            {
                LastName = user.LastName,

                FirstName = user.FirstName,
                Department = user.Department,
                Position = user.Position,
                Email = user.Email,
                UserName = user.Email,
                IsActive = true,
                IsDeleted = false

                
            };
            var result = await this.userManager.CreateAsync(User, password);

            
            var error = result.ToString();
            if (result.Succeeded)
            {
                var claimDepartment = new Claim("Department", User.Department);
                var claimPosition = new Claim("Position", User.Position);
                await this.userManager.AddClaimAsync(User, claimDepartment);
                await this.userManager.AddClaimAsync(User, claimPosition);
                return User;
            }
            else return error;
        }

        public async Task<ActionResult<Object>> Login(LoginUserDto loginUser)
        {

            var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, false);
            if (result.Succeeded)

            {
                var currentUser = userManager.Users.FirstOrDefault(u => u.Email == loginUser.Email);


                var claims = await userManager.GetClaimsAsync(currentUser);
                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                var token = CreateToken(claims, expiresAt);
                //Console.ReadLine();
                return new JwtToken
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt
                };
            }

            else return "User Not Found";

        }




        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}





