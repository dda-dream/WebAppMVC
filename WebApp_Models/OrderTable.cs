using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMVC_Models;

namespace WebApp_Models
{
    public class OrderTable
    {
        [Key]
        public int Id { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }



        [DisplayName("Дата заказа")]
        public DateTime OrderDate { get; set; }


        [Required]
        [DisplayName("Номер телефона")]
        public string PhoneNumber {get; set; }


        [Required]
        [DisplayName("Полное имя")]
        public string FullName {get; set; }


        [Required]
        [DisplayName("Емейл")]
        public string Email {get; set; }


    }
}
