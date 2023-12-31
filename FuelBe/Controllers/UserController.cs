﻿using FuelBe.Database;
using FuelBe.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuelBe.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly ReservationDbContext dbContext;
        private readonly IUserResolver userResolver;

        public UserController(ReservationDbContext dbContext, IUserResolver userResolver) {
            this.dbContext = dbContext;
            this.userResolver = userResolver;
        }

        [HttpPost("register")]
        public IActionResult RegisterNewUser(Database.Models.User user) {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("admin")]
        public IActionResult IsAdmin() {
            return Ok(userResolver.IsAdmin);
        }

        [HttpGet("info")]
        public ActionResult<Database.Models.User> GetUser(int id) {
            var findUser = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if (findUser == null) {
                //TODO
            }
            return Ok(findUser);
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<Database.Models.User>> GetAll() {
            return dbContext.Users.ToList();
        }
    }
}
