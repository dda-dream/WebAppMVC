using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC_Models
{
    public class LogTableModel
    {
        [Key]
        public int Id {get; set;}

        [DisplayName("Дата и время создания")]
        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [DisplayName("Log type: (insert/update/delete)")]
        public string TypeStr { get; set; }


        [Required]
        [DisplayName("LogTableName:")]
        public string LogTableName { get; set; }
        [Required]
        [DisplayName("LogRecordId:")]
        public int LogRecordId { get; set; }



        [DisplayName("Message:")]
        public string? Message { get; set; }
    }
}
