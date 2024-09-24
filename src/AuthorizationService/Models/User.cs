using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
