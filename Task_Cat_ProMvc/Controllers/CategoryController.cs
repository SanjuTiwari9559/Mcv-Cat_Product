

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Models.viewModel;
using Task_Cat_ProMvc.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task_Cat_ProMvc.Controllers
{
    public class CategoryController : Controller
    {

       Uri baseUrl = new Uri("https://localhost:7220/api");
        private readonly ICategory newcategory;
       private readonly HttpClient client;

        public CategoryController(ICategory category)
        {
            this.newcategory = category;
            this.client = new HttpClient();
            client.BaseAddress = baseUrl;
        }


        [HttpGet]
        public async Task<IActionResult> List(int pageNo=1,int PageSize=10)
        {
            var categories = await newcategory.GetAllCategoriesAsync(pageNo,PageSize);
            HttpResponseMessage responce = client.GetAsync(client.BaseAddress + "/Category/GetCategoriesAsync").Result;
            if (responce.IsSuccessStatusCode)
            {
                string Data = responce.Content.ReadAsStringAsync().Result;

                return View(JsonConvert.DeserializeObject<List<Product>>(Data));
            }
            return Ok(null);
          

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult>Add(addCategoryRequest addCategoryRequest)
        {
            var categry = new Category
            {
                Name = addCategoryRequest.Name,
                IsActive = addCategoryRequest.IsActive,
            };
            await newcategory.AddCategoryAsync(categry);
            return Redirect("List");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category=await newcategory.GetByIdAsync(id);
            if (category != null)
            {
                var editRequest = new editCategoryRequest
                {
                    Id = id,
                    IsActive = category.IsActive,
                    Name = category.Name,
                };
                return View(editRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult>Edit( editCategoryRequest editCategoryRequest)
        {
            var category = new Category
            {
                Name = editCategoryRequest.Name,
                IsActive = editCategoryRequest.IsActive,
                 Id = editCategoryRequest.Id
            };
        var result=await  newcategory.UpdateCategoryAsync(editCategoryRequest.Id,category);
            return RedirectToAction("List");



        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteViewModel  deleteViewModel)
        {
          var result= await  newcategory.DaleteCategoryAsyn(deleteViewModel.Id);
            if(result==true)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
      
    }
}
