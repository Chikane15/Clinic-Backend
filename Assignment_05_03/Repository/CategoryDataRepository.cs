using Assignment_05_03.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Assignment_05_03.Repository
{

    public class CategoryDataRepository : IDataRespository<Category, int>
    {
        EshoppingDbContext ctx;

        CollectionRespons<Category> collection = new CollectionRespons<Category>();
        SingleObjectRespons<Category> single = new SingleObjectRespons<Category>();

        public CategoryDataRepository(EshoppingDbContext ctx)
        {
            this.ctx = ctx;
        }

        public Task<CollectionRespons<Product>> GetProducts(string CatName, string condition, string manufacturer)
        {
            throw new NotImplementedException();
        }

        public Task<CollectionRespons<Product>> GetProductsByCategoryName(string CatName)
        {
            throw new NotImplementedException();
        }

        async Task<SingleObjectRespons<Category>> IDataRespository<Category, int>.CreateAsync(Category entity)
        {
            try
            {
                if (await IsCategoryIdUnique(entity.CategoryId))
                {
                    var result = await ctx.Categories.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Category Record is added successfully";
                    single.StatusCode = 200; 
                }
                else
                {
                    single.Message = "Category with similar id is present";
                    single.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                single.Message = ex.Message;
                single.StatusCode = 500;
            }
            return single;
        }

        async Task<SingleObjectRespons<Category>> IDataRespository<Category, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Categories.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Category based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Categories.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Category Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionRespons<Category>> IDataRespository<Category, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Categories.ToListAsync();
                collection.Message = "All Categories are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectRespons<Category>> IDataRespository<Category, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Categories.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Category based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Category Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectRespons<Category>> IDataRespository<Category, int>.UpdateAsync(int id, Category entity)
        {
            try
            {
                var rec = await ctx.Categories.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Category based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.CategoryId = entity.CategoryId;
                rec.CategoryName = entity.CategoryName;
                rec.BasePrice = entity.BasePrice;
                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Category Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsCategoryIdUnique(string categoryId)
        {
            return !await ctx.Categories.AnyAsync(c => c.CategoryId == categoryId);
        }
    }
}
