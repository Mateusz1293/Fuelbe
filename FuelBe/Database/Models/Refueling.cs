using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("refueling", Schema = "dbo")]
    public class Refueling {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("vehicle_id")]
        public int VehicleId { get; set; }
        [Column("quantity")]
        public decimal Quantity { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("counter_status")]
        public int CounterStatus { get; set; }
        [Column("add_date")]
        public DateTime AddDate { get; set; }
        //----------------------------------------------------------------
        public User? User { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
