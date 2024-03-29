using Assignment_05_03.Models;
using Assignment_05_03.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_05_03.Controllers
{

    [Route("api/[controller]")]

    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {


        IDataRespository<Product, int> proRepo;

        public ProductController(IDataRespository<Product, int> repo)
        {
            proRepo = repo;
        }

        [HttpGet]
        //[Authorize(Policy = "GetPolicy")]
        async public Task<IActionResult> Get()
        {
            var response = await proRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
       // [Authorize(Policy = "GetPolicy")]
        async public Task<IActionResult> Get(int id)
        {
            var response = await proRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
       // [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Post(Product product)
        {
            if (ModelState.IsValid) {
                var response = await proRepo.CreateAsync(product);
                return Ok(response);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
      //  [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Put(int id, Product product)
        {
            var response = await proRepo.UpdateAsync(id, product);
            return Ok(response);
        }
        [HttpDelete("{id}")]
       // [Authorize(Policy = "DeletePolicy")]
        async public Task<IActionResult> Delete(int id)
        {
            var response = await proRepo.DeleteAsync(id);
            return Ok(response);
        }

    }
}
