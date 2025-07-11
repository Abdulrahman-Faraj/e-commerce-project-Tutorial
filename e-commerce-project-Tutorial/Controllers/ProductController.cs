using e_commerce_project_Tutorial.Data;
using e_commerce_project_Tutorial.Models;
using e_commerce_project_Tutorial.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_project_Tutorial.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _imagesFolderPath;
        private const int PageSize = 3;

        public ProductController(EcommerceDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _imagesFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "Product-Image");
            if (!Directory.Exists(_imagesFolderPath))
            {
                Directory.CreateDirectory(_imagesFolderPath);
            }

        }
        public async Task<IActionResult> Index(string? searchName, string? searchBrand, decimal? maxPrice
            , decimal? minPrice, int page = 1)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(p => p.Name.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchBrand))
            {
                query = query.Where(p => p.Brand.Contains(searchBrand));
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            var totalRecords = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            var Products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var productDisplayList = Products.Select(p => new ProductDisplayViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                Description = p.Description,
                Discount = p.Discount,
                MainImageFileName = p.MainImageFileName,
            }).ToList();
            var vm = new ProductListViewModel
            {
                Products = productDisplayList,
                SearchName = searchName,
                SearchBrand = searchBrand,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                CurrentPage = page,
                TotalPages = totalPages

            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var product = await _context.Products.AsNoTracking()
                .Include(p => p.RelatedImages)
                .FirstOrDefaultAsync(p => p.ProductId == Id);

            if (product == null)
            {
                return NotFound();
            }
            var vm = new ProductDisplayViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Description = product.Description,
                Discount = product.Discount,
                MainImageFileName = product.MainImageFileName,
                RelatedImage = product.RelatedImages
                .Select(img => new ProductRelatedImageDisplayViewModel
                {
                    ImageId = img.ImageId,
                    ImageFileName = img.ImageFileName
                }).ToList()
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var product = new Product
            {
                Name = vm.Name,
                Brand = vm.Brand,
                Price = vm.Price,
                Description = vm.Description,
                Discount = vm.Discount,

            };
            if (vm.MainImageFile != null && vm.MainImageFile.Length > 0)
            {
                using var memoryStrem = new MemoryStream();
                await vm.MainImageFile.CopyToAsync(memoryStrem);
                var fileByte = memoryStrem.ToArray();
                product.MainImageData = fileByte;
                var uniqueFileName = GenerateUniqueFileName(vm.MainImageFile.FileName);
                product.MainImageFileName = uniqueFileName;
                var filePath = Path.Combine(_imagesFolderPath, uniqueFileName);
                await System.IO.File.WriteAllBytesAsync(filePath, fileByte);

            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (vm.RelatedImageFiles != null && vm.RelatedImageFiles.Any())
            {
                foreach (var file in vm.RelatedImageFiles)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        var fileByte = memoryStream.ToArray();
                        var productImage = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageData = fileByte
                        };
                        var uniqueFileName = GenerateUniqueFileName(file.FileName);
                        productImage.ImageFileName = uniqueFileName;
                        var relatedFilePath = Path.Combine(_imagesFolderPath, uniqueFileName);
                        await System.IO.File.WriteAllBytesAsync(relatedFilePath, fileByte);
                        _context.ProductImages.Add(productImage);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.AsNoTracking()
                .Include(p => p.RelatedImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var vm = new UpdateProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                ExistingMainImageFileName = product.MainImageFileName,
                ExistingRelatedImage = product.RelatedImages.ToList(),
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var product = await _context.Products
                .Include(p => p.RelatedImages)
                .FirstOrDefaultAsync(p => p.ProductId == vm.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = vm.Name;
            product.Brand = vm.Brand;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.Discount = vm.Discount;

            if (vm.MainImageFile != null && vm.MainImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.MainImageFileName))
                {
                    var oldPath = Path.Combine(_imagesFolderPath, product.MainImageFileName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                }
                var uniqueFileName = GenerateUniqueFileName(vm.MainImageFile.FileName);
                product.MainImageFileName = uniqueFileName;
                var filePath = Path.Combine(_imagesFolderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await vm.MainImageFile.CopyToAsync(fileStream);
                }
                product.MainImageData = System.IO.File.ReadAllBytes(filePath);
            }
            if (vm.RelatedImageFiles != null && vm.RelatedImageFiles.Any())
            {
                foreach (var file in vm.RelatedImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = GenerateUniqueFileName(file.Name);
                        var productImage = new ProductImage
                        {
                            ProductId = product.ProductId,
                            ImageFileName = uniqueFileName

                        };
                        var filePath = Path.Combine(_imagesFolderPath, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        productImage.ImageData = System.IO.File.ReadAllBytes(filePath);
                        _context.ProductImages.Add(productImage);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product  = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p=>p.ProductId == id);

            if(product == null)
            {
                return NotFound();  
            }
            var vm = new DeleteproductViewModel
            {
                ProductId = product.ProductId,
                Name =  product.Name,
                Brand = product.Brand,
                Price = product.Price,
                MainImageFileName = product.MainImageFileName,
                Description = product.Description,

            };
            return View(vm);    
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products
                .Include(p => p.RelatedImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) { return NotFound(); }
            if (!string.IsNullOrEmpty(product.MainImageFileName))
            {
                var filePath = Path.Combine(_imagesFolderPath, product.MainImageFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            foreach (var img in product.RelatedImages)
            {
                var path = Path.Combine(_imagesFolderPath, img.ImageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.ProductImages.RemoveRange(product.RelatedImages);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteImages(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null) { return NotFound(); }
            var imagePath = Path.Combine(_imagesFolderPath,image.ImageFileName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = image.ProductId });
        }           
       private string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return Guid.NewGuid().ToString() + extension;
        }
    }


}

