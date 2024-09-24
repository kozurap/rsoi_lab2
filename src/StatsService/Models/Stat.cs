using System.ComponentModel.DataAnnotations.Schema;

namespace StatsService.Models
{
    [Table("stat")]
    public class Stat
    {
            [Column("id")]
            public int Id { get; set; }

            [Column("text")]
            public string Text { get; set; }
    }
}
