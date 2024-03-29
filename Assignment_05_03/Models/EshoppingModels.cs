using Assignment_05_03.Customization.Validators;
using System.ComponentModel.DataAnnotations;

namespace Assignment_05_03.Models
{
    public abstract class EntityBase { }
    public class Category : EntityBase
    {

        [Key]
        public int CategoryUniqueId { get; set; }
        [Required]
        [LengthCustomization(ErrorMessage = "Category ID should be less than 15 charcaters")]
        public string? CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }

        public int BasePrice { get; set; }
        // Tell the Category That the Empty Product List is present 
        // for the newly added category
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    }

    public class Product : EntityBase
    {
        [Key]
        public int ProductUniqueId { get; set; }
        
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Manufacturer { get; set; }
        
        public int Price { get; set; }
        public int CategoryUniqueId { get; set; }

        //public Category Category { get; set; } = new Category();
    }
}
