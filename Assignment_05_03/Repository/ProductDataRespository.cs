using Assignment_05_03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Assignment_05_03.Repository
{
    public class ProductDataRespository : IDataRespository<Product, int>
    {
        EshoppingDbContext ctx;

        CollectionRespons<Product> collection = new CollectionRespons<Product>();
        SingleObjectRespons<Product> single = new SingleObjectRespons<Product>();

        public ProductDataRespository(EshoppingDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectRespons<Product>> IDataRespository<Product, int>.CreateAsync(Product entity)
        {
            try
            {
                if (Isless(entity))
                {
                    var result = await ctx.Products.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Product Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Product price must be more then category base price";
                    single.StatusCode=200;
                }
            }
            catch (Exception ex)
            {
                single.Message= ex.Message;
                single.StatusCode = 500;
            }
            return single;
        }

        async Task<SingleObjectRespons<Product>> IDataRespository<Product, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Products.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Product based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Products.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Products Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionRespons<Product>> IDataRespository<Product, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Products.ToListAsync();
                collection.Message = "All Prtoducts are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectRespons<Product>> IDataRespository<Product, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Products.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Product based on Id={id} is not found";
                    single.StatusCode = 500;
                   
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Product Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectRespons<Product>> IDataRespository<Product, int>.UpdateAsync(int id, Product entity)
        {
            try
            {
                var rec = await ctx.Products.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Product based on Id={id} is not found";
                    single.StatusCode = 500;

                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.Manufacturer = entity.Manufacturer;

                rec.ProductName = entity.ProductName;
                rec.Price = entity.Price;
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

        public async Task<CollectionRespons<Product>> GetProductsByCategoryName(string CatName)
        {
            try
            {
                var category = await ctx.Categories.Where(c => c.CategoryName == CatName).ToListAsync();
                int id = 0;
                foreach (var item in category)
                {
                    id = item.CategoryUniqueId;
                }
                collection.Records = await ctx.Products.Where(p => p.CategoryUniqueId == id).ToListAsync();
                if (!collection.Records.Any())
                {
                    collection.Message = "No such products";
                    collection.StatusCode = 200;
                }
                else
                {
                    collection.Message = "All Prtoducts are read successfully";
                    collection.StatusCode = 200;
                }
                return collection;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
            
        }


        public async Task<CollectionRespons<Product>> GetProducts(string CatName, string condition, string manufacturer)
        {

            try
            {
                if (condition == "AND")
                {
                    collection.Records = (from c in ctx.Categories
                                          join p in ctx.Products on c.CategoryUniqueId equals p.CategoryUniqueId
                                          where c.CategoryName == CatName && p.Manufacturer == manufacturer
                                          select p).ToList();
                   /* var cat= ctx.Categories.Where(c=>c.CategoryName== CatName).First();
                    collection.Records=await ctx.Products.Where(p=>p.Manufacturer==manufacturer && p.CategoryUniqueId==cat.CategoryUniqueId).ToListAsync();*/
                    if (!collection.Records.Any())
                    {
                        collection.Message = "No such products";
                        collection.StatusCode = 200;
                    }
                    else
                    {
                        collection.Message = "All Prtoducts are read successfully";
                        collection.StatusCode = 200;
                    }
                    return collection;
                }
                else if(condition == "OR")
                {
                    collection.Records = (from c in ctx.Categories
                                          join p in ctx.Products on c.CategoryUniqueId equals p.CategoryUniqueId
                                          where c.CategoryName == CatName || p.Manufacturer == manufacturer
                                          select p).ToList();
                    if (!collection.Records.Any())
                    {
                        collection.Message = "No such products";
                        collection.StatusCode = 200;
                    }
                    else
                    {
                        collection.Message = "All Prtoducts are read successfully";
                        collection.StatusCode = 200;
                    }
                    return collection;
                }
               
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;

            /* if (condition == "AND")
             {
                 var category = (await cat.GetAsync()).Records.Where(c => c.CategoryName == CatName);
                 foreach (var item in category)
                 {
                     id = item.CategoryUniqueId;

                 }
                 var prod = (await prd.GetAsync()).Records.Where(p => p.CategoryUniqueId == id);
                 foreach (var item in prod)
                 {
                     var result = (await prd.GetAsync()).Records.Where(p => p.Manufacturer==manufacturer);
                 }
             }
             */
           

        }

        public bool Isless(Product p)
        {
            
            var cat=ctx.Categories.Where(pc=>pc.CategoryUniqueId == p.CategoryUniqueId).FirstOrDefault();
           
               
                if (cat.BasePrice < p.Price)
                {
                    return true;
                }
                else
                {
                    return false;
                }
          
        }
    }
}
