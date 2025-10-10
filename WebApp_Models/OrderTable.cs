using System;
using System.Collections.Generic;
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


        public DateTime OrderDate { get; set; }

        [Required]
        public string PhoneNumber {get; set; }
        [Required]
        public string FullName {get; set; }
        [Required]
        public string Email {get; set; }


    }
}
