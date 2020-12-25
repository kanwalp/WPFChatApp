using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApplication.Model
{
    [Table("dbo.Users")]
    public class Users
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string IconName { get; set; }
    }
}
