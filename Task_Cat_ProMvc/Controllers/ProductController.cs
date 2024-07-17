using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Models.viewModel;
using Task_Cat_ProMvc.Services;

namespace Task_Cat_ProMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _productService;
        private readonly ICategory _categoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProduct productService, ICategory categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var products = await _productService.GetAllAsync(pageNumber, pageSize);
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product list.");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync(1, int.MaxValue);
                var addProductRequest = new AddProductRequest
                {
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList()
                };
                return View(addProductRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories for add view.");
                return View("Error");
            }
        }
        [HttpPost]
       
        public async Task<IActionResult> Add(AddProductRequest addProduct)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var categories = await _categoryService.GetAllCategoriesAsync(1, int.MaxValue);
                    addProduct.Categories = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching categories for add view.");
                    return View("Error");
                }
                return View(addProduct);
            }

            try
            {
                var product = new Product
                {
                    Name = addProduct.Name,
                    IsActive = addProduct.IsActive,
                    CategoryId = addProduct.CategoryId
                };

                await _productService.AddAsync(product);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product.");
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var categories = await _categoryService.GetAllCategoriesAsync(1, int.MaxValue);
                var editProduct = new EditProductRequest
                {
                    CategoryId = product.CategoryId,
                    Id = id,
                    IsActive = product.IsActive,
                    Name = product.Name,
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList()
                };

                ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
                return View(editProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product for edit view.");
                return View("Error");
            }
        }



                
        public async Task<IActionResult> Edit(EditProductRequest editProduct)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(editProduct);
            //}

            try
            {
                var product = new Product
                {
                    Id = editProduct.Id,
                    Name = editProduct.Name,
                    CategoryId = editProduct.CategoryId,
                    IsActive = editProduct.IsActive
                };

                var result = await _productService.UpdateAsync(product, editProduct.Id);
                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to update product.");
                    return View(editProduct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product.");
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteProductRequest deleteProduct)
        {
            try
            {
                var result = await _productService.DeleteAsync(deleteProduct.Id);
                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to delete product.");
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product.");
                return View("Error");
            }
        }
    }
}

