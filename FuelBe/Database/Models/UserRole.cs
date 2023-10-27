using System.ComponentModel.DataAnnotations.Schema;

namespace FuelBe.Database.Models {
    [Table("userRole", Schema = "dbo")]
    public class UserRole {
        [Column("userId")]
        public int UserId { get; set; }
        [Column("roleId")]
        public int RoleId { get; set; }
        //--------------------------------------
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
