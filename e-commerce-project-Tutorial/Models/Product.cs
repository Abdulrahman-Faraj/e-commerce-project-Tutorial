using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce_project_Tutorial.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Brand { get; set; }

        [Range(0,double.MaxValue)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0,100)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Discount { get; set; }

        public string? MainImageFileName { get; set; }
        public byte[]? MainImageData { get; set; }
        public ICollection<ProductImage> RelatedImages   { get; set; }
    }
}


