using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class MyDailyJournal
    {
        [Key]
        public int Id { get; set; }
 
        [DisplayName("Дата и время:")]
        public DateTime LogDate { get; set; }

        [DisplayName("Сообщение:")]
        public string? Text { get; set; }
    }
}
