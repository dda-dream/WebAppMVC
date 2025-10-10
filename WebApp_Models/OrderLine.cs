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
    public class OrderLine
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderTable OrderTable { get; set; }



        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }



        [DisplayName("Количество")]
        public int Qty { get; set; }
    }
}
