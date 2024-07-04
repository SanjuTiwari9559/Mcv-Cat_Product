using Microsoft.AspNetCore.Mvc;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Models.viewModel;
using Task_Cat_ProMvc.Services;

namespace Task_Cat_ProMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct product;

        public ProductController(IProduct product)
        {
            this.product = product;
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber=1,int pageSize=10)
        {
            var products =await product.GetAllAsync(pageNumber, pageSize);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Add(AddProductRequest addCategory)
        {
            var products = new Product
            {
                Name = addCategory.Name,
                IsActive = addCategory.IsActive,
                CategoryId = addCategory.CategoryId
            };
             await product.AddAsync(products);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var products=await product.GetByIdAsync(id);
            if(products != null)
            {
                var editProduct = new EditProductRequest
                {
                    Id = id,
                    IsActive = products.IsActive,
                    Name = products.Name,
                    CategoryId = products.CategoryId
                };
                return View(editProduct);
            }
            return RedirectToAction("List");

        }
        [HttpPost]
        public async Task<IActionResult>Edit(EditProductRequest editProduct)
        {
            var products = new Product
            {
                Id = editProduct.Id,
                Name = editProduct.Name,
                CategoryId = editProduct.CategoryId,
                IsActive = editProduct.IsActive,

            };
             var result= await product.UpdateAsync(products,editProduct.Id);
            if (result == true)
            {
                return RedirectToAction("List");
            }
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteProductRequest deleteProduct)
        {
             var result= await product.DeleteAsync(deleteProduct.Id);
            if (result == true)
            {
                return RedirectToAction("List");
            }
            return View(result);
        }
    }
}
