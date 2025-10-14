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
    public class SalesLine
    {
        [Key]
        public int Id { get; set; }



        [Required]
        public int SalesId { get; set; }
        [ForeignKey("SalesId")]
        public SalesTable SalesTable { get; set; }


        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        public int Qty { get; set; }
        public double Price { get; set; }


    }
}
