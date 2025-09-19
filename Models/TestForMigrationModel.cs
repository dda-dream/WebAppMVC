using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class TestForMigrationModel
    {
        [Key]
        public int Id { get; set; }
 

        [DisplayName("Message:")]
        public string? Text_1 { get; set; }
    }
}
