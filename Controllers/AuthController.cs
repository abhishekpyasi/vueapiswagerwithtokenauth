using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vueapi.Dtos.UserDto;
using Vueapi.Model;
using Vueapi.Service;

namespace Vueapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private AuthService authService { get; }

        public AuthController(IConfiguration configuration, AuthService authService)
        {
            this.configuration = configuration;
            this.authService = authService;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] UserDto user)
        {

            var registeredUser = await authService.Register(user);

            if (registeredUser.Value.GetType() == typeof(User))
                return Ok("User Added Successfully");
            else
            {
                string temp = registeredUser.Value.ToString();
                string Result = temp.Replace("UserName", " User Email");
                //Result = Result.Replace("InvalidEmail", "Email Already Exists");
                return BadRequest(Result);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserDto user)
        {

            var token = await authService.Login(user);


            return Ok(token.Value);

        }
    }





}
