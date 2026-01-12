using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_DataAccess.Repository.DTO
{
    public class ProductSelector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public double Price { get; set; }
    }
}
