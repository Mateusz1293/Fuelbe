using FuelBe.Database;
using FuelBe.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FuelBe.Controllers {
    [Route("api/refueling")]
    [ApiController]
    public class ReservationController : ControllerBase {
        private readonly ReservationDbContext dbContext;

        public ReservationController(ReservationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpPost("add-reservation")]
        public ActionResult<int> CheckIsFree(AddReservationDto addResDto) {
            var addRes = new Reservation {
                VehicleId = addResDto.VehicleId,
                UserId = addResDto.UserId,
                DateFrom = addResDto.DateFrom,
                DateTo = addResDto.DateTo
            };
           var addedRes = dbContext.Reservations.Add(addRes);
            dbContext.SaveChanges();
            return addedRes.Entity.Id;
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

        [HttpDelete("deleteReservation")]
        public ActionResult DeleteReservation(int reservationId) {
            var gerReservation = dbContext.Reservations.Where(x => x.Id == reservationId).FirstOrDefault();
            if (gerReservation != null) {
                dbContext.Reservations.Remove(gerReservation);
                dbContext.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("get-reservation")]
        public ActionResult<Reservation> GetReservation(int reservationId) {
            var getReservation = dbContext.Reservations
              .Where(x => x.Id == reservationId)
              .Include(x => x.User)
              .Include(x => x.Vehicle)
              .Select(x => new Reservation {
                  Id = x.Id,
                  DateFrom = x.DateFrom,
                  DateTo = x.DateTo,
                  User = new User {
                      Id = x.User != null ? x.User.Id : 0,
                      FirstName = x.User != null ? x.User.FirstName : "",
                      LastName = x.User != null ? x.User.LastName : "",
                      Login = x.User != null ? x.User.Login : ""
                  },
                  Vehicle = new Vehicle {
                      Id = x.Vehicle != null ? x.Vehicle.Id : 0,
                      Model = x.Vehicle != null ? x.Vehicle.Model : ""
                  }
              })
              .FirstOrDefault();
            return Ok(getReservation);
        }

        [HttpGet("get-car-calendar")]
        public ActionResult<List<ReservationDto>> GetCarValendar(int carId) {
            var dateNow = DateTime.Now;
            var getReservations = dbContext.Reservations
                .Where(x => x.VehicleId == carId && x.DateTo >= dateNow)
                .Include(x => x.User)
                .OrderBy(x => x.DateTo)
                .Select(x => new Reservation {
                    Id = x.Id,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    User = new User {
                        Id = x.User != null ? x.User.Id : 0,
                        FirstName = x.User != null ? x.User.FirstName : "",
                        LastName = x.User != null ? x.User.LastName : "",
                        Login = x.User != null ? x.User.Login : ""
                    }
                })
                .ToList();
            var resList = new List<ReservationDto>();
            if (getReservations.Count == 0) {
                resList.Add(new ReservationDto { DateFrom = dateNow, DateTo = new DateTime(9999,12,31), IsFree = true });
                return resList;
            } else {
                DateTime lastDay = DateTime.Now;
                getReservations.ForEach(x => {
                    if (resList.Count == 0) {
                        if (x.DateFrom > dateNow) {
                            resList.Add(new ReservationDto { DateFrom = dateNow, DateTo = x.DateFrom.AddDays(-1), IsFree = true});
                        }
                        resList.Add(new ReservationDto { DateFrom = x.DateFrom <= dateNow ? dateNow : x.DateFrom, DateTo = x.DateTo, IsFree = false, Id = x.Id, User = x.User });
                    } else {
                        if (x.DateFrom.AddDays(-1) != resList.Last().DateTo) {
                            resList.Add(new ReservationDto { DateFrom = resList.Last().DateTo.AddDays(1), DateTo = x.DateFrom.AddDays(-1), IsFree = true });
                        }
                        resList.Add(new ReservationDto { DateFrom = x.DateFrom, DateTo = x.DateTo, IsFree = false, Id = x.Id, User = x.User });
                    }
                    lastDay = x.DateTo;
                });
                resList.Add(new ReservationDto { DateFrom = lastDay.AddDays(1), DateTo = new DateTime(9999,12,31), IsFree = true });
                return resList;
            }
        }

        public class ReservationDto {
            public int? Id { get; set; }
            public DateTime DateFrom  { get; set; }
            public DateTime DateTo { get; set; }
            public bool IsFree { get; set; }
            public User? User { get; set; }
        }

        public class AddReservationDto {
            public int VehicleId { get; set; }
            public int UserId { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
        }
    }
}
