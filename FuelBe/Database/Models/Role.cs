using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("role", Schema = "dbo")]
    public class Role {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("description")]
        public string? Description { get; set; }
        //-------------------------------------------------
        public IEnumerable<UserRole>? UsersRoles { get; set; }
    }
}
