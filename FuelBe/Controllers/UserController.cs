using FuelBe.Database;
using FuelBe.Database.Models;
using FuelBe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static FuelBe.Controllers.AuthController;
using static FuelBe.Controllers.UserController;

namespace FuelBe.Controllers {
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
            var findUser = dbContext.Users.Where(x => x.Id == id)
                .FirstOrDefault();
            var findUser2 = new UserData();
            if (findUser != null) {
                findUser2.Id = findUser.Id;
                findUser2.Login = findUser.Login;
                findUser2.Email = findUser.Email;
                findUser2.FirstName = findUser.FirstName;
                findUser2.LastName = findUser.LastName;
                findUser2.IsAdmin = false;
            }
            var getRoleAdmin = dbContext.UsersRoles
                .Where(xs => xs.UserId == id && (xs.Role != null && xs.Role.Name == "ADMIN"))
                .FirstOrDefault();
            if (getRoleAdmin != null) {
                findUser2.IsAdmin = true;
            }
            return Ok(findUser2);
        }

        public class ChangePassword {
            public string OldPassword { get; set; } = string.Empty;
            public int UserId { get; set; }
        }

        [HttpPost("checkOldPassword")]
        public ActionResult<object> CheckIsOldPasswordIsCorrect(ChangePassword oldPass) {
            var findByLogin = dbContext.Users.Where(x => x.Id == oldPass.UserId).FirstOrDefault();
            if (findByLogin == null) {
                return Ok(new { IsCorrect = false });
            }
            string savedPasswordHash = findByLogin.Password;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(oldPass.OldPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++) {
                if (hashBytes[i + 16] != hash[i]) {
                    return Ok(new { IsCorrect = false });
                }
            }
            return Ok(new { IsCorrect = true });
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangeToNewPassword(ChangePassword oldPass) {
            var findByLogin = dbContext.Users.Where(x => x.Id == oldPass.UserId).FirstOrDefault();
            if (findByLogin != null) {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(oldPass.OldPassword, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string hashedPassword = Convert.ToBase64String(hashBytes);

                findByLogin.Password = hashedPassword;
                dbContext.Users.Update(findByLogin);
                dbContext.SaveChanges();
                return Ok();
            }
            return Ok();
        }

            [HttpGet("users")]
        public ActionResult<IEnumerable<Database.Models.User>> GetAll() {
            var resultUsers = new List<UserData>();
            var getAll = dbContext.Users
                .ToList();
            getAll.ForEach(x => {
                var getRoleAdmin = dbContext.UsersRoles
                    .Where(xs => xs.UserId == x.Id && (xs.Role != null && xs.Role.Name == "ADMIN"))
                    .FirstOrDefault();
                var userData = new UserData {
                    Id = x.Id,
                    Login = x.Login,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Password = x.Password,
                    IsAdmin = getRoleAdmin != null ? true : false
                };
                resultUsers.Add(userData);
            });
            return Ok(resultUsers);
        }

        [HttpPut("userUpdate")]
        public ActionResult<Database.Models.User> UpdateUserData(UserUpdate userUpdate) {
            var findUser = dbContext.Users.Where(x => x.Id == userUpdate.Id).FirstOrDefault();
            if (findUser != null) {
                if (findUser.Login != userUpdate.Login) {
                    findUser.Login = userUpdate.Login;
                }
                if (findUser.Email != userUpdate.Email) {
                    findUser.Email = userUpdate.Email;
                }
                if (userUpdate.Password.Length > 0) {
                    byte[] salt;
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(userUpdate.Password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    string hashedPassword = Convert.ToBase64String(hashBytes);
                    findUser.Password = hashedPassword;
                }
                if (findUser.FirstName != userUpdate.FirstName) {
                    findUser.FirstName = userUpdate.FirstName;
                }
                if (findUser.LastName != userUpdate.LastName) {
                    findUser.LastName = userUpdate.LastName;
                }
                dbContext.Users.Update(findUser);
                dbContext.SaveChanges();
                var getAdminRoleId = dbContext.Roles.Where(x => x.Name == "ADMIN").FirstOrDefault();
                if (getAdminRoleId != null) {
                    var getUserRoleAdmin = dbContext.UsersRoles.Where(x => x.RoleId == getAdminRoleId.Id && x.UserId == userUpdate.Id).FirstOrDefault();
                    if (getUserRoleAdmin != null && !userUpdate.IsAdmin) {
                        dbContext.UsersRoles.Remove(getUserRoleAdmin);
                        dbContext.SaveChanges();
                    } else if (getUserRoleAdmin == null && userUpdate.IsAdmin) {
                        var newUserRole = new UserRole {
                            UserId = userUpdate.Id,
                            RoleId = getAdminRoleId.Id
                        };
                        dbContext.UsersRoles.Add(newUserRole);
                        dbContext.SaveChanges();
                    }
                }
            }
            return Ok(findUser);
        }

        [HttpDelete("userDelete")]
        public ActionResult DeleteUser(int userId) {
            var findUser = dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (findUser != null) {
                dbContext.Users.Remove(findUser);
                dbContext.SaveChanges();
            }
            return Ok();
        }

        [HttpPost("userAdd")]
        public IActionResult AddUser(UserAdd userAdd) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(userAdd.Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPassword = Convert.ToBase64String(hashBytes);

            var addUser = new User {
                Login = userAdd.Login,
                Email = userAdd.Email,
                FirstName = userAdd.FirstName,
                LastName = userAdd.LastName,
                Password = hashedPassword
            };
            var addedUser = dbContext.Users.Add(addUser);
            dbContext.SaveChanges();
            if (userAdd.IsAdmin) {
                var getAdminRole = dbContext.Roles
                    .Where(x => x.Name == "ADMIN")
                    .FirstOrDefault();
                if (getAdminRole != null) {
                    var userRole = new UserRole {
                        UserId = addedUser.Entity.Id,
                        RoleId = getAdminRole.Id
                    };
                    dbContext.UsersRoles.Add(userRole);
                    dbContext.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpPut("currUserUpdate")]
        public ActionResult<Database.Models.User> UpdateCurrUserData(CurrUserUpdate userUpdate) {
            var findUser = dbContext.Users.Where(x => x.Id == userUpdate.Id).FirstOrDefault();
            if (findUser != null) {
                if (findUser.Email != userUpdate.Email) {
                    findUser.Email = userUpdate.Email;
                }
                if (findUser.FirstName != userUpdate.FirstName) {
                    findUser.FirstName = userUpdate.FirstName;
                }
                if (findUser.LastName != userUpdate.LastName) {
                    findUser.LastName = userUpdate.LastName;
                }
                dbContext.Users.Update(findUser);
                dbContext.SaveChanges();
            }
            return Ok(findUser);
        }

        public class CurrUserUpdate {
            public int Id { get; set; }
            public string Email { get; set; } = string.Empty;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        public class UserAdd {
            public string Login { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string Password { get; set; } = string.Empty;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class UserUpdate {
            public int Id { get; set; }
            public string Login { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string Password { get; set; } = string.Empty;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class UserData {
            public int Id { get; set; }
            public string Login { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string Password { get; set; } = string.Empty;
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public bool IsAdmin { get; set; }
        }
    }
}
