using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC_Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        

        [Required(ErrorMessage = "Category Name должно быть заполнено")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        

        [Required(ErrorMessage = "Display Order должно быть заполнено")]
        [Range(1,int.MaxValue, ErrorMessage = "Значение Display Order должно быть больше 0")] 
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
