using FuelBe.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using FuelBe.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Reflection.Metadata.Ecma335;

namespace FuelBe.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly ReservationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserResolver userResolver;

        public AuthController(ReservationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IUserResolver userResolver) {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userResolver = userResolver;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto) {
            bool isAdmin = false;
            //znajdź żytkownika o takim loginie i haśle
            var findByLogin = dbContext.Users.Where(x => x.Login == loginDto.Login).FirstOrDefault();
            if (findByLogin == null) {
                return Ok(new AuthMessage() { Status = 404, Message = "Użytkownik nie istnieje", IsAdmin = false});
            }
            if (findByLogin.Password != loginDto.Password) {
                return Ok(new AuthMessage() { Status = 404, Message = "Hasło jest niepopranwe", IsAdmin = false });
            }
            //jeśli użytkownik istnieje znajdź jego role
            var getRole = dbContext.UsersRoles
                .Where(x => x.UserId == findByLogin.Id)
                .Include(x => x.Role)
                .ToList();
            var keyBytes = Encoding.UTF8.GetBytes("this is my custom Secret key for authentication");
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(
                symmetricKey,
                // 👇 one of the most popular. 
                SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>();
            //var roleClaims = permissions.Select(x => new Claim("role", x));
            // claims.AddRange(roleClaims);

            getRole.ForEach(x =>
            {
                if (x.Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, x.Role.Name));
                    if (x.Role.Name == "ADMIN")
                    {
                        isAdmin = true;
                    }
                }
            });

            claims.Add(new Claim("id", findByLogin.Id.ToString()));
            claims.Add(new Claim("admin", isAdmin.ToString()));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7295",
                audience: "https://localhost:7295",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new AuthMessage() { Token = rawToken, Status = 200, Message = "Zalogowano!", IsAdmin = isAdmin, Id = findByLogin.Id });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync() {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                return Unauthorized();
            }
            await httpContextAccessor.HttpContext.SignOutAsync();
            return Ok();
        }



        [HttpGet("getLogInfo")]
        public IActionResult GetLogInfo(string token) {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = token;
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "id").Value;
            var isAdmin = tokenS.Claims.First(claim => claim.Type == "admin").Value;
            return Ok(new DataInfo { Id = Convert.ToInt32(id), IsAdmin = bool.Parse(isAdmin) });
        }

        [HttpGet("is-logged")]
        public IActionResult IsLogged() {
            var userId = userResolver.Id;
            if (userId != 0) {
                return Ok(new AuthMessage() { Status = 200});
            }
            return Ok(new AuthMessage() { Status = 401 });
        }
        //---------------------------------------Modele

        public class LoginDto
        {
            public string Login { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class AuthMessage {
            public int? Status { get; set; }
            public string? Message { get; set; }
            public bool? IsAdmin { get; set; }
            public int? Id { get; set; }
            public string? Token { get; set; }
        }

        public class DataInfo {
            public int? Id { get; set; }
            public bool? IsAdmin { get; set; }
        }
    }
}
