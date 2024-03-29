using Assignment_05_03.Models;
using Assignment_05_03.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_05_03.Controllers
{
  
    [Route("api/[controller]")]

   // [Authorize]
    [ApiController]
    
    public class CategoryController : Controller
    {


        IDataRespository<Category, int> catRepo;
        IDataRespository<Product, int> ppRepo;

        public CategoryController(IDataRespository<Category, int> repo, IDataRespository<Product, int> pRepo)
        {
            catRepo = repo;
            ppRepo = pRepo;

        }

        [HttpGet]
       // [Authorize(Policy = "GetPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> Get()
        {
            var response = await catRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
       // [Authorize(Policy = "GetPolicy")]

        async public Task<IActionResult> Get(int id)
        {
            var response = await catRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        //[Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Post(Category cat)
        {
            var response = await catRepo.CreateAsync(cat);
            return Ok(response);
        }


        [HttpPut("{id}")]
        //[Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Put(int id, Category cat)
        {
            var response = await catRepo.UpdateAsync(id, cat);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "DeletePolicy")]
        async public Task<IActionResult> Delete(int id)
        {
            var response = await catRepo.DeleteAsync(id);
            return Ok(response);
        }


        [HttpPost("CreateWithProduct")]
       // [Authorize]
        async public Task<IActionResult> PostByCategory(Category cat)
        {
            var response = await catRepo.CreateAsync(cat);


            foreach (var products in cat.Products)
            {
                products.CategoryUniqueId = response.Record.CategoryUniqueId;
                await ppRepo.CreateAsync(products);
            }
            return Ok(response);
        }
    }
}
