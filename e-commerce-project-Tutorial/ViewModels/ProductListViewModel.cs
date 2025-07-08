namespace e_commerce_project_Tutorial.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<ProductDisplayViewModel> Products { get; set; }

        public string SearchName { get; set; }
        public string SearchBrand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }


}
