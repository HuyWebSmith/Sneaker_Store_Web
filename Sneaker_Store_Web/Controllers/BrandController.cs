using Microsoft.AspNetCore.Mvc;
using Sneaker_Store_Web.Models;
using Sneaker_Store_Web.Repositories;

namespace Sneaker_Store_Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        public BrandController(IProductRepository productRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
        }

        public async Task<IActionResult> Index()
        {
            var brand = await _brandRepository.GetAllAsync();
            return View(brand);
        }

        public async Task<IActionResult> Display(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        public IActionResult Add()  
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Brand brand, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Tạo tên file duy nhất để tránh trùng lặp
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/brand_images", fileName);

                    // Lưu file vào thư mục wwwroot/images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn vào database
                    brand.ImageUrl = "/brand_images/" + fileName;
                }
                if (ModelState.IsValid)
                {
                    await _brandRepository.AddAsync(brand);
                    return RedirectToAction(nameof(Index));
                }             
            }
            return View(brand);
        }

        public async Task<IActionResult> Update(int id)
        {


            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Brand brand, IFormFile? ImageFile)
        {
            var existingBrand = await _brandRepository.GetByIdAsync(id);
            if (existingBrand == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Nếu có ảnh mới được tải lên
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Xóa ảnh cũ (nếu có)
                    if (!string.IsNullOrEmpty(existingBrand.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingBrand.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Tạo file mới
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/brand_images", fileName);

                    // Lưu file mới vào thư mục wwwroot/brand_images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Cập nhật đường dẫn ảnh
                    brand.ImageUrl = "/brand_images/" + fileName;
                }
                else
                {
                    // Nếu không chọn ảnh mới, giữ nguyên ảnh cũ
                    brand.ImageUrl = existingBrand.ImageUrl;
                }

                // Cập nhật thông tin khác của thương hiệu
                existingBrand.BrandName = brand.BrandName;
                existingBrand.ImageUrl = brand.ImageUrl;

                await _brandRepository.UpdateAsync(existingBrand);
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand != null)
            {
                await _brandRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
