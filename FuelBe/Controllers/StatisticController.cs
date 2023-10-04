using FuelBe.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
