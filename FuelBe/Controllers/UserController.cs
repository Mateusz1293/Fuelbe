using FuelBe.Database;
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

        [HttpGet("info")]
        public ActionResult<Database.Models.User> GetUser() {
            var findUser = dbContext.Users.Where(x => x.Id == userResolver.getId()).FirstOrDefault();
            if (findUser == null) {
                throw new Exception("Nie ma usera");
            }
            return Ok(findUser);
        }

        [HttpPost("login")]
        public ActionResult<object> Login(LoginDto loginDto) {
            var findByLogin = dbContext.Users.Where(x => x.Login == loginDto.Login).FirstOrDefault();
            if (findByLogin == null) {
                throw new Exception("Użytkownik nie istnieje w bazie");
            }
            if (findByLogin.Password != loginDto.Password) {
                throw new Exception("Hasło jest niepopranwe");
            }
            userResolver.setId(findByLogin.Id);
            return Ok(new { token = findByLogin.Id });
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<Database.Models.User>> GetAll() {
            return dbContext.Users.ToList();
        }

        //--------------------------------------obiekty
        public class LoginDto
        {
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
