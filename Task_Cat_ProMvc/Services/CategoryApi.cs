using Newtonsoft.Json;
using System.Text;
using Task_Cat_ProMvc.Models.Data;

namespace Task_Cat_ProMvc.Services
{
    public class CategoryApi : ICategory
    {
        private readonly Cat_ProductDbContext cat_ProductDbContextApi;

        private readonly HttpClient _httpClient;

        public CategoryApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task ActivateCategoryAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Category/Activate/{id}", null);
            response.EnsureSuccessStatusCode();
        }


        public async Task AddCategoryAsync(Category category)
        {
            var categoryJson = JsonConvert.SerializeObject(category);
            var response = await _httpClient.PostAsync("api/Category", new StringContent(categoryJson, Encoding.Default, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DaleteCategoryAsyn(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Category/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task DeactivateCategoryAsync(int id)
        {
            var response = await _httpClient.PutAsync($"api/Category/Deactivate/{id}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync($"api/Category?pageNumber={pageNumber}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(content);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Category/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Category>(content);
        }
        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            var categoryJson = JsonConvert.SerializeObject(category);
            var response = await _httpClient.PutAsync($"api/Category/{id}", new StringContent(categoryJson, Encoding.Default, "application/json"));
            return response.IsSuccessStatusCode;
        }

       

        
    }
}
