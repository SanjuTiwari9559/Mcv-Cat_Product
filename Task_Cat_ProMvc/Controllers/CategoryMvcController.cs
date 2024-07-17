

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Models.viewModel;
using Task_Cat_ProMvc.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task_Cat_ProMvc.Controllers
{
    public class CategoryMvcController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICategory _categoryService;
        private readonly bool _useApi;

        public CategoryMvcController(ICategory category, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _categoryService = category;
            _clientFactory = httpClientFactory;
            _useApi = configuration.GetValue<bool>("UseApi");
        }

        [HttpGet]
        public async Task<IActionResult> List(int pageNo = 1, int pageSize = 10)
        {
            if (_useApi)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5290/api/CategoryApi?pageNo={pageNo}&pageSize={pageSize}");
                var jsonData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Category>>(jsonData);
                return View(apiResponse.Categories);
            }
                
            
            else
            {
                var categories = await _categoryService.GetAllCategoriesAsync(pageNo, pageSize);
                return View(categories);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(addCategoryRequest addCategoryRequest)
        {
            var category = new Category
            {
                Name = addCategoryRequest.Name,
                IsActive = addCategoryRequest.IsActive
            };

            if (_useApi)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5290/api/CategoryApi", category);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
            }
            else
            {
                await _categoryService.AddCategoryAsync(category);
                return RedirectToAction("List");
            }
            return View(addCategoryRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (_useApi)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"http://localhost:5290/api/CategoryApi/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<Category>(jsonData);
                    var editRequest = new editCategoryRequest
                    {
                        Id = id,
                        IsActive = category.IsActive,
                        Name = category.Name
                    };
                    return View(editRequest);
                }
            }
            else
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category != null)
                {
                    var editRequest = new editCategoryRequest
                    {
                        Id = id,
                        IsActive = category.IsActive,
                        Name = category.Name
                    };
                    return View(editRequest);
                }
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(editCategoryRequest editCategoryRequest)
        {
            var category = new Category
            {
                Id = editCategoryRequest.Id,
                Name = editCategoryRequest.Name,
                IsActive = editCategoryRequest.IsActive
            };

            if (_useApi)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PutAsJsonAsync($"http://localhost:5290/api/CategoryApi/{editCategoryRequest.Id}", category);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
            }
            else
            {
                await _categoryService.UpdateCategoryAsync(editCategoryRequest.Id, category);
                return RedirectToAction("List");
            }
            return View(editCategoryRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(deleteViewModel deleteViewModel)
        {
            if (_useApi)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync($"http://localhost:5290/api/CategoryApi/{deleteViewModel.Id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
            }
            else
            {
                var result = await _categoryService.DaleteCategoryAsyn(deleteViewModel.Id);
                if (result)
                {
                    return RedirectToAction("List");
                }
            }
            return RedirectToAction("Edit");
        }
    }
}

        //private readonly IHttpClientFactory _clientFactory;
        //private readonly bool _useApi;


        //Uri baseUrl = new Uri("http://localhost:5290//api/Category");
       // private readonly ICategory _categoryService;
        //private readonly Cat_ProductDbContext cat_ProductDbContext;
        //private readonly bool _useApi=true ; 
        //private readonly bool _useMvc=true ;
        //private readonly bool apiaction;


        // private readonly HttpClient client;


        //public CategoryMvcController(ICategory category, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        //{
        //    this._categoryService = category;
        //    _clientFactory = httpClientFactory;
        //    _useApi = configuration.GetValue<bool>("UseApi");

        //    //  this.client = new HttpClient();


        //    //client.BaseAddress = baseUrl;
        //}



    //    [HttpGet]
    //    public async Task<IActionResult> List(int pageNo = 1, int PageSize = 10)
    //    {
    //        //var APIorMVC= cat_ProductDbContext.CallApi.Where(x => x.IsActive == true);
    //        //  if(APIorMVC.Any())
    //        // {
    //        //var response = await client.GetAsync($"{baseUrl}?pageNo={pageNo}&pageSize={PageSize}");
    //        //if (response.IsSuccessStatusCode)
    //        //{
    //        //    var jsonData = await response.Content.ReadAsStringAsync();
    //        //    var newcategory = JsonConvert.DeserializeObject<List<Category>>(jsonData);
    //        //    return View(newcategory);

    //        //}
    //        //var response = await _client.GetAsync($"{_baseUrl}?pageNo={pageNo}&pageSize={PageSize}");
    //        //response.EnsureSuccessStatusCode();

    //        //var jsonData = await response.Content.ReadAsStringAsync();
    //        //var categories = JsonConvert.DeserializeObject<List<Category>>(jsonData);

    //        //return View(categories);
    //        //var categories = await newcategory.GetAllCategoriesAsync(pageNo, PageSize);
    //        //return View(categories);
    //        if (_useApi)
    //        {
    //            var client = _clientFactory.CreateClient();
    //            var response = await client.GetAsync($"http://localhost:5290/api/CategoryApi?pageNo={pageNo}&pageSize={PageSize}");
    //            if (response.IsSuccessStatusCode)
    //            {
    //                var jsonData = await response.Content.ReadAsStringAsync();
    //                var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonData);
    //                return View(categories);
    //            }
    //            return View(new List<Category>());
    //        }
    //        else
    //        {
    //            var categories = await _categoryService.GetAllCategoriesAsync(pageNo, PageSize);
    //            return View(categories);
    //        }
    //    }

    //}



    //    //var categories = await newcategory.GetAllCategoriesAsync(pageNo, PageSize);
    //    //HttpResponseMessage responce = client.GetAsync(client.BaseAddress + "/Category/GetCategoriesAsync").Result;
    //    //if (responce.IsSuccessStatusCode)
    //    //{
    //    //    string Data = responce.Content.ReadAsStringAsync().Result;

    //    //    return View(JsonConvert.DeserializeObject<List<Product>>(Data));
    //    //}
    //    // return View(categories);





    //   // [HttpGet]
    ////    public async Task<IActionResult> Add()
    ////    {
    ////        return View();
    ////    }
    ////    [HttpPost]
    ////    public async Task<IActionResult> Add(addCategoryRequest addCategoryRequest)
    ////    {
    ////        //var categry = new Category
    ////        //{
    ////        //    Name = addCategoryRequest.Name,
    ////        //    IsActive = addCategoryRequest.IsActive,
    ////        //};
    ////        //await newcategory.AddCategoryAsync(categry);
    ////        //return Redirect("List");
    ////        var category = new Category
    ////        {
    ////            Name = addCategoryRequest.Name,
    ////            IsActive = addCategoryRequest.IsActive,
    ////        };

    ////        var jsonContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
    ////        var response = await _client.PostAsync(_baseUrl, jsonContent);
    ////        response.EnsureSuccessStatusCode();

    ////        return RedirectToAction("List");

    ////    }
    ////    [HttpGet]
    ////    public async Task<IActionResult> Edit(int id)
    ////    {
    ////        //var category=await newcategory.GetByIdAsync(id);
    ////        //if (category != null)
    ////        //{
    ////        //    var editRequest = new editCategoryRequest
    ////        //    {
    ////        //        Id = id,
    ////        //        IsActive = category.IsActive,
    ////        //        Name = category.Name,
    ////        //    };
    ////        //    return View(editRequest);
    ////        //}
    ////        //return View(null);
    ////        var response = await _client.GetAsync($"{_baseUrl}/{id}");
    ////        response.EnsureSuccessStatusCode();

    ////        var jsonData = await response.Content.ReadAsStringAsync();
    ////        var category = JsonConvert.DeserializeObject<Category>(jsonData);

    ////        var editRequest = new editCategoryRequest
    ////        {
    ////            Id = id,
    ////            IsActive = category.IsActive,
    ////            Name = category.Name,
    ////        };
    ////        return View(editRequest);
    ////    }
    ////    [HttpPost]
    ////    public async Task<IActionResult> Edit(editCategoryRequest editCategoryRequest)
    ////    {
    ////        //    var category = new Category
    ////        //    {
    ////        //        Name = editCategoryRequest.Name,
    ////        //        IsActive = editCategoryRequest.IsActive,
    ////        //         Id = editCategoryRequest.Id
    ////        //    };
    ////        //var result=await  newcategory.UpdateCategoryAsync(editCategoryRequest.Id,category);
    ////        //    return RedirectToAction("List");
    ////        var category = new Category
    ////        {
    ////            Id = editCategoryRequest.Id,
    ////            Name = editCategoryRequest.Name,
    ////            IsActive = editCategoryRequest.IsActive
    ////        };

    ////        var jsonContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
    ////        var response = await _client.PutAsync($"{_baseUrl}/{editCategoryRequest.Id}", jsonContent);
    ////        response.EnsureSuccessStatusCode();

    ////        return RedirectToAction("List");


    ////    }
    ////    [HttpPost]
    ////    public async Task<IActionResult> Delete(deleteViewModel deleteViewModel)
    ////    {
    ////        //  var result= await  newcategory.DaleteCategoryAsyn(deleteViewModel.Id);
    ////        //    if(result==true)
    ////        //    {
    ////        //        return RedirectToAction("List");
    ////        //    }
    ////        //    return RedirectToAction("Edit");
    ////        //}
    ////        var response = await _client.DeleteAsync($"{_baseUrl}/{deleteViewModel.Id}");
    ////        response.EnsureSuccessStatusCode();

    ////        return RedirectToAction("List");

    ////    }
    ////}

