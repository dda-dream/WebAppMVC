using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC_Models
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



        [DisplayName("Дата и время создания")]
        [Required]
        public DateTime CreatedDateTime { get; set; }

        [DisplayName("Дата и время изменения")]
        [Required]
        public DateTime ModifiedDateTime { get; set; } 

        public int Test { get; set; }
    }
}
