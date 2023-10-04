using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("user", Schema = "dbo")]
    public class User {
        [Column("id")]
        public int Id { get; set; }
        [Column("login")]
        public string Login { get; set; } = string.Empty;
        [Column("email")]
        public string? Email { get; set; }
        [Column("password")]
        public string Password { get; set; } = string.Empty;
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        //-----------------------------
        public IEnumerable<Reservation>? Reservations { get; set; }
        public IEnumerable<Refueling>? Refuelings { get; set; }
    }
}
