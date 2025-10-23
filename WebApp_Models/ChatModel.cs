using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp_Utility;


namespace WebAppMVC_Models
{
    public class ChatModel
    {

        [Key]
        public int Id { get; set; }
 

        public DateTime MessageDate { get; set; }


        public string MessageUserNickName { get; set; }
        public string MessageText { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
