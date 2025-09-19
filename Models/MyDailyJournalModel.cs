using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class MyDailyJournalModel
    {
        [Key]
        public int Id { get; set; }
 

        [Required(ErrorMessage = "Дата и время должно быть заполнено")]
        [DisplayName("Дата и время")]
        public DateTime LogDate { get; set; }


        [Required(ErrorMessage = "Сообщение должно быть заполнено")]
        [DisplayName("Сообщение")]
        public string Text { get; set; }
    }
}
