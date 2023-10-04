using FuelBe.Database;
using FuelBe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace FuelBe.Controllers {
    [Route("api/vahicle")]
    [ApiController]
    public class VehicleController : ControllerBase {
        private readonly ReservationDbContext dbContext;
        private readonly IUserResolver userResolver;

        public VehicleController(ReservationDbContext dbContext, IUserResolver userResolver) {
             this.dbContext = dbContext;
             this.userResolver = userResolver;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<Database.Models.Vehicle>> GetAll() {
            return dbContext.Vehicles.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Database.Models.Vehicle> GetById(int id) {
            var getVehicle = dbContext.Vehicles
                .Where(x => x.Id == id)
                .FirstOrDefault();
            if (getVehicle == null) {
                throw new Exception("Pojazd nie istnieje");
            }
            return getVehicle;
        }

        [HttpPost("add")]
        public IActionResult AddVehicle(Database.Models.Vehicle vehicle) {
            dbContext.Vehicles.Add(vehicle);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("get-curr-user-car")]
        public ActionResult<Database.Models.Vehicle> GetCurrentUserVehicle() {
            var getCar = dbContext.Vehicles
                .Where(x => x.Reservations != null && x.Reservations
                    .Any(xs => xs.UserId == userResolver.getId() && xs.DateFrom <= DateTime.Now && xs.DateTo >= DateTime.Now))
                .FirstOrDefault();
            if (getCar == null) {
                return NotFound();
            }
            return Ok(getCar);
        }
    }
}
