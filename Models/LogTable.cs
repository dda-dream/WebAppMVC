using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class LogTable
    {
        [Key]
        public int Id {get; set;}

        [DisplayName("Дата и время создания")]
        [Required]
        public DateTime CreatedDateTime { get; set; }


        [DisplayName("Log type: (insert/update/delete)")]
        public string TypeStr { get; set; }



        [DisplayName("Source:")]
        public string Source { get; set; }



        [DisplayName("Message:")]
        public string? Message { get; set; }
    }
}
