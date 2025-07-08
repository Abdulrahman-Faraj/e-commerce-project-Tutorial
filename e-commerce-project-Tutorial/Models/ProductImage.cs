global using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce_project_Tutorial.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public int ProductId { get; set; }
        
        public string ImageFileName { get; set; }

        public byte[] ImageData { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}


