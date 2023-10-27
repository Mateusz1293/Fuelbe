using FuelBe.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace FuelBe.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly ReservationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthController(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor) {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto) {
            //znajdź żytkownika o takim loginie i haśle
            var findByLogin = dbContext.Users.Where(x => x.Login == loginDto.Login).FirstOrDefault();
            if (findByLogin == null) {
                throw new Exception("Użytkownik nie istnieje w bazie");
            }
            if (findByLogin.Password != loginDto.Password) {
                throw new Exception("Hasło jest niepopranwe");
            }
            //jeśli użytkownik istnieje znajdź jego role
            var getRole = dbContext.UsersRoles
                .Where(x => x.UserId == findByLogin.Id)
                .Include(x => x.Role)
                .ToList();
            //wpisz najważniejsze dane do claimsów
            var claims = new List<Claim>(); //pusta lista do której będą zapisywane różne informacje o aktualnie zalogowanym użytkoniku
            //dodać do listy identyfikator użytkownika
            claims.Add(new Claim(ClaimTypes.NameIdentifier, findByLogin.Id.ToString()));
            //dodać do list imię i nzawisko
            claims.Add(new Claim(ClaimTypes.Name, $"{findByLogin.FirstName} {findByLogin.LastName}"));
            //dodać do listy role
            getRole.ForEach(x => {
                if (x.Role != null) {
                    claims.Add(new Claim(ClaimTypes.Role, x.Role.Name));
                }
            });
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties {
                RedirectUri = "/Home/Index",
            };
            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });
            return Ok(claims);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync() {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                return Unauthorized();
            }
            await httpContextAccessor.HttpContext.SignOutAsync();
            return Ok();
        }

        //---------------------------------------Modele

        public class LoginDto
        {
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
