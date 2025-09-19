using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAppMVC.Models
{
    public class ProductModel
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}
        public string Description {get; set;}

        [Range(1, int.MaxValue)]
        public double Price {get; set;}
        public string Image {get; set;}

        [Display(Name="Category Type")]
        public int CategoryId {get; set;}
        [ForeignKey("CategoryId")]
        public virtual CategoryModel Category {get; set;}
    }
}
