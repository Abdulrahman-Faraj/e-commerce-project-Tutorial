namespace e_commerce_project_Tutorial.ViewModels
{
    public class ProductDisplayViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }

        public decimal Discount { get; set; }
        public string? MainImageFileName { get; set; }
        public List<ProductRelatedImageDisplayViewModel> RelatedImage { get; set; } = new();

    }
    public class ProductRelatedImageDisplayViewModel
    {
        public int ImageId { get; set; }
        public string ImageFileName { get; set; }
    }



}
