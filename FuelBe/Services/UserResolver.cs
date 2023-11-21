using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FuelBe.Services {
    public class UserResolver : IUserResolver {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserResolver(IHttpContextAccessor httpContextAccessor) {
                this.httpContextAccessor = httpContextAccessor;
        }

        public int Id {
            get {
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userId != null) {
                    return Convert.ToInt32(userId.Value);
                }
                return 0;
            }
        }

        public bool IsAdmin {
            get {
                var isA = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role);
                bool isAdmin = false;
                isA.ToList().ForEach(x => {
                    if (x.Value == "ADMIN") {
                        isAdmin = true;
                       // return true;
                    }
                });
                return isAdmin;
            }
        }

        public bool IsLogged => throw new NotImplementedException();



        public void setId(int id)
        {
            return;
        }

        public int getId()
        {
            return this.Id;
        }
    }
}
