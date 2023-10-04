using FuelBe.Database;
using Microsoft.AspNetCore.Mvc;

namespace FuelBe.Controllers {
    [Route("api/refueling")]
    [ApiController]
    public class RefuelingController : ControllerBase {
        private readonly ReservationDbContext dbContext;

        public RefuelingController(ReservationDbContext dbContext) {
            this.dbContext = dbContext;
        }


    }
}
