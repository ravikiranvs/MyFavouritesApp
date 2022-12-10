using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFavouritesEntities.Models;
using MyFavouritesRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFavouritesAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class SignInController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IConfiguration _configRoot;
        private readonly IUserRepository _userRepository;

        public SignInController(ILogger<WeatherForecastController> logger, IUserRepository userRepo, IConfiguration configRoot, IDataProtectionProvider dataProtectionProvider, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepo = userRepo;
            _dataProtectionProvider = dataProtectionProvider;
            _configRoot = configRoot;
            _userRepository = userRepository;
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] SignInInfo signInInfo)
        {
            var secret = _configRoot.GetSection("secret").Value;


            var user = await _userRepository.GetUserByUserNameAsync(signInInfo.userName);

            if(user != null)
            {
                var password = _dataProtectionProvider.CreateProtector(secret).Unprotect(user.Password);

                if (password == signInInfo.password)
                {
                    var issuer = _configRoot["Jwt:Issuer"];
                    var audience = _configRoot["Jwt:Audience"];
                    var key = Encoding.ASCII.GetBytes(_configRoot["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id", user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(5),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    var stringToken = tokenHandler.WriteToken(token);
                    return Ok(stringToken);
                }
            }

            return Unauthorized();
        }

        [Route("signup")]
        [HttpPost]
        public async Task Create([FromBody] SignInInfo signInInfo)
        {
            var secret = _configRoot.GetSection("secret").Value;

            var passwordHash = _dataProtectionProvider.CreateProtector(secret).Protect(signInInfo.password);

            var user = new User {
                Naunce = string.Empty,
                Password = passwordHash,
                Salt = string.Empty,
                UserName = signInInfo.userName
            };

            var userId = await _userRepository.UpdateOrCreateUserByIdAsync(user);

            var userClaims = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            userClaims.AddClaim(new Claim("userId", user.Id.ToString()));
            var userPr = new ClaimsPrincipal(userClaims);
            await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, userPr);

            return;
        }
    }

    public class SignInInfo
    {
        public string userName { get; set; }

        public string password { get; set; }
    }
}

