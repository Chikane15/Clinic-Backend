using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        IDataRepositoy<PatientModel, int> patRepo;
        IDataRepositoy<DoctorModel, int> docRepo;

        public DoctorController(IDataRepositoy<PatientModel, int> patRepo, IDataRepositoy<DoctorModel, int> docRepo)
        {
            this.patRepo = patRepo;
            this.docRepo = docRepo;

        }

        [HttpGet]
        [Authorize(Policy = "GetPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> Get()
        {
            var response = await docRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetPolicy")]

        async public Task<IActionResult> Get(int id)
        {
            var response = await docRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Post(DoctorModel doctor)
        {
            var response = await docRepo.CreateAsync(doctor);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Put(int id, DoctorModel doctor)
        {
            var response = await docRepo.UpdateAsync(id, doctor);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "DeletePolicy")]
        async public Task<IActionResult> Delete(int id)
        {
            var response = await docRepo.DeleteAsync(id);
            return Ok(response);
        }
    }
}
