using e_commerce_project_Tutorial.Models;

namespace e_commerce_project_Tutorial.ViewModels
{
    public class UpdateProductViewModel
    {
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Brand { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public decimal Discount { get; set; }

        public IFormFile? MainImageFile { get; set; }

        public List<IFormFile>? RelatedImageFiles { get; set; } = new();
        public string? ExistingMainImageFileName { get; set; }
        public List<ProductImage>? ExistingRelatedImage { get; set; } = new();

    }


}
