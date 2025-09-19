using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class ForTestMigrationModel
    {
        [Key]
        public int Id { get; set; }
 

        [DisplayName("Message:")]
        public string? Text_Renamed { get; set; }
    }
}
