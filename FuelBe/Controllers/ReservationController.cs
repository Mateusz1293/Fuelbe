using FuelBe.Database;
using Microsoft.AspNetCore.Mvc;

namespace FuelBe.Controllers {
    [Route("api/refueling")]
    [ApiController]
    public class ReservationController : ControllerBase {
        private readonly ReservationDbContext dbContext;

        public ReservationController(ReservationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet("get-days-to-end")]
        public int GetDaysToEnd(int userId, int carId) {
            var currDate = DateTime.Now;
            var getDate = dbContext.Reservations
                .Where(x => x.VehicleId == carId && x.UserId == userId && (x.DateFrom <= currDate && x.DateTo >= currDate))
                .FirstOrDefault();
            if (getDate is not null) {
                var days = getDate.DateTo - currDate;
                return days.Days;
            }
            return 0;
        }
    }
}
