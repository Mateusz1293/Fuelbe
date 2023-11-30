using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("vehicle", Schema = "dbo")]
    public class Vehicle {
        [Column("id")]
        public int Id { get; set; }
        [Column("model")]
        public string Model { get; set; } = string.Empty;
        [Column("register_number")]
        public string RegisterNumber { get; set; } = string.Empty;
        [Column("vin")]
        public string Vin { get; set;} = string.Empty;
        [Column("vehicle_type")]
        public string VehicleType { get; set; } = string.Empty;
        [Column("gearbox")]
        public string Gearbox { get; set; } = string.Empty;
        [Column("production_year")]
        public int ProductionYear { get; set; }
        [Column("fuel_type")]
        public string FuelType { get; set; } = string.Empty;
        [Column("insurance_to")]
        public DateTime InsuranceTo { get; set; }
        [Column("inspection_to")]
        public DateTime InspectionTo { get; set; }
        [Column("tires_type")]
        public string TiresType { get; set; } = string.Empty;
        //-----------------------------
        public IEnumerable<Reservation>? Reservations { get; set; }
        public IEnumerable<Refueling>? Refuelings { get; set; }
    }
}
