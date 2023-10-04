using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("reservation", Schema = "dbo")]
    public class Reservation {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("vehicle_id")]
        public int VehicleId { get; set; }
        [Column("date_from")]
        public DateTime DateFrom { get; set; }
        [Column("date_to")]
        public DateTime DateTo { get; set; }
        //----------------------------------------------------------------
        public User? User { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
