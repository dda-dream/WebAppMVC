using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class MyDailyJournal
    {
        [Key]
        public int Id { get; set; }
 

        [DisplayName("Дата и время")]
        [Required(ErrorMessage = "Дата и время должно быть заполнено")]
        public DateTime LogDate { get; set; }

        [DisplayName("Сообщение")]
        [Required(ErrorMessage = "Сообщение должно быть заполнено")]
        public string Text { get; set; }
    }
}
