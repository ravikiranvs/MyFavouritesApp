using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFavouritesEntities;
using MyFavouritesEntities.Models;
using MyFavouritesRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFavouritesAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserRepository _userRepo;
        public UserController(ILogger<WeatherForecastController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }
        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{userName}")]
        public async Task<User> Get()
        {
            string userName = "";
            var user = await _userRepo.GetUserByUserNameAsync(userName);
            return user;
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] User user)
        {
            if (_userRepo.GetUserByUserNameAsync(user.UserName) == null)
            {
                await _userRepo.UpdateOrCreateUserByIdAsync(user);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] User user)
        {
            user.Id = id;
            await _userRepo.UpdateOrCreateUserByIdAsync(user);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _userRepo.DeleteUserByIdAsync(id);
        }
    }
}

