using FuelBe.Database;
using FuelBe.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace FuelBe.Controllers {
    [Route("api/refueling")]
    [ApiController]
    public class RefuelingController : ControllerBase {
        private readonly ReservationDbContext dbContext;

        public RefuelingController(ReservationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet("get-actual-counter-status")]
        public decimal GetActualCounterStatus(int carId) {
            var data = dbContext.Refuelings
                .Where(x => x.VehicleId == carId)
                .OrderByDescending(x => x.AddDate)
                .ToList();
            return data.Count > 0 ? data.First().CounterStatus : 0M;
        }

        [HttpPost("add-refueling")]
        public void AddRefueling(Refueling refueling) {
            refueling.AddDate = DateTime.Now;
            dbContext.Refuelings.Add(refueling);
            dbContext.SaveChanges();
            Ok();
        }

        [HttpGet("get-user-refueling-all")]
        public ActionResult<List<Refueling>> GetUserRefuelingAll(int userId, int carId) {
            var getRefueling = dbContext.Refuelings
                .Where(x => x.VehicleId == carId && x.UserId == userId)
                .OrderByDescending(x => x.AddDate)
                .ToList();
            return Ok(getRefueling);
        }

        [HttpGet("getUserCar")]
        public ActionResult<List<object>> GetUserCar(int userId) {
            var getReservations =  dbContext.Reservations
                .Where(x => x.UserId == userId && (x.DateFrom <= DateTime.Now && x.DateTo >= DateTime.Now))
                .ToList();
            List<object> cars = new List<object>();
            getReservations.ForEach(x => {
                var getCar = dbContext.Vehicles.Where(xs => xs.Id == x.VehicleId).FirstOrDefault();
                var getDays = (x.DateTo - x.DateFrom).TotalDays;
                if (getCar != null) {
                    cars.Add(new 
                    {
                        Id = getCar.Id,
                        Model = getCar.Model,
                        RegisterNumber = getCar.RegisterNumber,
                        TotalDays = getDays
                    });
                }
            });
            return Ok(cars);
        }
    }
}
