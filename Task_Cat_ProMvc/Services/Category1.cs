using Microsoft.EntityFrameworkCore;
using Task_Cat_ProMvc.Models.Data;

namespace Task_Cat_ProMvc.Services
{
    public class Category1 : ICategory
    {
        private readonly Cat_ProductDbContext cat_ProductDb;

        public Category1(Cat_ProductDbContext cat_ProductDb)
        {
            this.cat_ProductDb = cat_ProductDb;
        }
        public async Task ActivateCategoryAsync(int id)
        {
            var category =  await cat_ProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = true;
                await cat_ProductDb.SaveChangesAsync();
                await ActivateProductsByCategoryIdAsync(id);

            }
        }

        public  async Task AddCategoryAsync(Category addCategory)
        {
            
            await cat_ProductDb.Categories.AddAsync(addCategory);
            await cat_ProductDb.SaveChangesAsync(); 
        }

        public async Task<bool> DaleteCategoryAsyn(int id)
        {
            var category = await cat_ProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                cat_ProductDb.Categories.Remove(category);
                await cat_ProductDb.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeactivateCategoryAsync(int id)
        {
            var category = await cat_ProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = false;
                await cat_ProductDb.SaveChangesAsync();
                await DeactivateProductsByCategoryIdAsync(id);

            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            return await cat_ProductDb.Categories.Include(c=>c.Products).Skip((1-pageNumber)*pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await cat_ProductDb.Categories.FindAsync(id);
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category Updatecategory)
        {
            var category = await cat_ProductDb.Categories.FindAsync(id);
            if (category != null)
            {


                category.Name = Updatecategory.Name;
                category.IsActive = Updatecategory.IsActive;
                //category.Products = Updatecategory.Products;

                await cat_ProductDb.SaveChangesAsync();
                return true;



            }
            return false;
        }
        private async Task ActivateProductsByCategoryIdAsync(int categoryId)
        {
            var products = await cat_ProductDb.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            foreach (var product in products)
            {
                product.IsActive = true;
                cat_ProductDb.Products.Update(product);
            }
            await cat_ProductDb.SaveChangesAsync();
        }
        private async Task DeactivateProductsByCategoryIdAsync(int categoryId)
        {
            var products = await cat_ProductDb.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            foreach (var product in products)
            {
                product.IsActive = false;
                cat_ProductDb.Products.Update(product);
            }
            await cat_ProductDb.SaveChangesAsync();
        }
    }
}
