using FuelBe.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FuelBe.Controllers {
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : ControllerBase {
        private readonly ReservationDbContext dbContext;

        public StatisticController(ReservationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet("months")]
        public ActionResult<object> GetSixMonths() {
            // create a dictionary 
            Dictionary<int, string> months = new Dictionary<int, string>();
            months.Add(1, "Styczeń");
            months.Add(2, "Luty");
            months.Add(3, "Marzec");
            months.Add(4, "Kwiecień");
            months.Add(5, "Maj");
            months.Add(6, "Czerwiec");
            months.Add(7, "Lipiec");
            months.Add(8, "Sierpień");
            months.Add(9, "Wrzesień");
            months.Add(10, "Październik");
            months.Add(11, "Listopad");
            months.Add(12, "Grudzień");
            List<string> results = new List<string>();
            for (int i = 5; i >= 0; i--){
                var getDate = DateTime.Now.AddMonths(i * (-1));
                var getMonth = getDate.Month;
                var getYear = getDate.Year;
                var getMonthName = months[getMonth];
                results.Add($"{getMonthName} {getYear}");
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public ActionResult<object> GetSumPriceByVehicleId(int id) {
            var getAllRefueling = dbContext.Refuelings
                .Where(x => x.VehicleId == id)
                .ToList();
            List<decimal> results = new List<decimal>();
            for (int i = 5; i >= 0; i--) {
                var getDate = DateTime.Now.AddMonths(i * (-1));
                var getMonth = getDate.Month;
                var getYear = getDate.Year;
                var getRefuelingByMonth = getAllRefueling
                    .Where(x => x.AddDate.Month == getMonth && x.AddDate.Year == getYear)
                    .ToList();
                var sumPrice = 0M;
                getRefuelingByMonth.ForEach(y => {
                    sumPrice = sumPrice + (y.Price * y.Quantity);
                });
                results.Add(sumPrice);
            }
            return Ok(results);
        }

        [HttpGet("CarFuel/{id}")]
        public ActionResult<object> GetCarFuelCount(int id) {
            var year = new List<int>();
            var fuelCount = new List<List<int>>();
            var initYear = DateTime.Now.Year;
            for (int i = 0; i < 5; i++) {
                var fuelByMonth = new List<int>();
                for (int j = 0; j < 12; j++) {
                    var dateFrom = new DateTime(initYear, j + 1, 1);
                    var dateTo = new DateTime(initYear, j + 1, DateTime.DaysInMonth(initYear, j+1));
                    var getRefuelig = dbContext.Refuelings
                        .Where(x => x.VehicleId == id && x.AddDate >= dateFrom && x.AddDate <= dateTo)
                        .OrderByDescending(x => x.AddDate)
                        .FirstOrDefault();
                    if (getRefuelig != null) {
                        fuelByMonth.Add(getRefuelig.CounterStatus);
                    } else {
                        //TODO: do obsłużenia
                        fuelByMonth.Add(0);
                    }
                }
                fuelCount.Insert(0, fuelByMonth);
                year.Insert(0, initYear);
                initYear--;
            }
            var result = new {
                Labels = year,
                FuelCount = fuelCount
            };
            return Ok(result);
        }

        [HttpGet("CarDayRes/{id}")]
        public ActionResult<object> GetCarDays(int id) {
            var year = new List<int>();
            var yesDay = new List<int>();
            var noDay = new List<int>();
            var initYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++) {
                var dayFrom = new DateTime(initYear, 1, 1);
                var dayTo = new DateTime(initYear, 12, 31);
                var totalDaysOfYear = dayTo.DayOfYear - dayFrom.DayOfYear;
                int totalDaysReservation = 0;
                var getReservation = dbContext.Reservations
                    .Where(x => x.VehicleId == id && x.DateFrom >= dayFrom && x.DateTo <= dayTo)
                    .ToList();
                getReservation.ForEach(x => {
                    var getDays = x.DateTo.AddDays(1).DayOfYear - x.DateFrom.DayOfYear;
                    totalDaysReservation += getDays;
                });
                year.Insert(0, initYear);
                yesDay.Insert(0, totalDaysReservation);
                noDay.Insert(0, totalDaysOfYear - totalDaysReservation);
                initYear--;
            }
            var result = new {
                Labels = year.ToArray(),
                YesData = yesDay.ToArray(),
                NoData = noDay.ToArray()
            };
            return Ok(result);
        }

        [HttpGet("burning/{id}")]
        public ActionResult<object> CarBurning(int id) {
            //weź kwartał
            var labelsGen = new List<GenerateLabels>();
            var getNowMonth = DateTime.Now.Month;
            var getNowYear = DateTime.Now.Year;
            //4x4
            for (int i = 0; i < 16; i++) {
                if (getNowMonth >= 1 && getNowMonth <=3 ) {
                    labelsGen.Insert(0, new GenerateLabels { Kwartal = 1, Year = getNowYear });
                } else if (getNowMonth >= 4 && getNowMonth <= 6) {
                    labelsGen.Insert(0, new GenerateLabels { Kwartal = 2, Year = getNowYear });
                } else if (getNowMonth >= 7 && getNowMonth <= 9) {
                    labelsGen.Insert(0, new GenerateLabels { Kwartal = 3, Year = getNowYear });
                } else if (getNowMonth >= 10 && getNowMonth <= 12) {
                    labelsGen.Insert(0, new GenerateLabels { Kwartal = 4, Year = getNowYear });
                }
                if (labelsGen.Count > 0) {
                    if (labelsGen.First().Kwartal == 1) {
                        getNowYear--;
                        getNowMonth = 11;
                    } else if (labelsGen.First().Kwartal == 2) {
                        getNowMonth = 2;
                    } else if (labelsGen.First().Kwartal == 3) {
                        getNowMonth = 5;
                    } else if (labelsGen.First().Kwartal == 4) {
                        getNowMonth = 8;
                    }
                }
            }
            return Ok(labelsGen);
        }

        public class GenerateLabels {
            public int Year { get; set; }
            public int Kwartal { get; set; }
        }
    }
}
