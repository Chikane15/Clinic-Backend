using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assignment_05_03.Models;
using Assignment_05_03.Repository;

namespace Assignment_05_03.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SearchAPIController : ControllerBase
    {
       
        IDataRespository<Product, int> prd;


        public SearchAPIController(IDataRespository<Product, int> prd)
        {
            
            this.prd = prd;
        }

           [HttpGet]
           [ActionName("GetProduct")]
        async public Task<IActionResult> Get(string CatName)
        {
            var response = await prd.GetProductsByCategoryName(CatName);
            return Ok(response);
        }




         [HttpGet("{CatName}/{condition}/{manufacturer}")]
         [ActionName("AndOr")]
        async public Task<IActionResult> Get(string CatName, string condition, string manufacturer)
        {
            var response = await prd.GetProducts(CatName,condition,manufacturer);
            return Ok(response);
        }

    }
}
