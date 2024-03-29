using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        IDataRepositoy<PatientModel, int> patRepo;
        IDataRepositoy<DoctorModel, int> docRepo;

        public PatientController(IDataRepositoy<PatientModel, int> patRepo, IDataRepositoy<DoctorModel, int> docRepo)
        {
           this.patRepo = patRepo;
            this.docRepo = docRepo;

        }

        [HttpGet]
        [Authorize(Policy = "GetPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> Get()
        {
            var response = await patRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetPolicy")]

        async public Task<IActionResult> Get(int id)
        {
            var response = await patRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Post(PatientModel patient)
        {
            var response = await patRepo.CreateAsync(patient);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "PostPutPolicy")]
        async public Task<IActionResult> Put(int id, PatientModel patient)
        {
            var response = await patRepo.UpdateAsync(id, patient);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "DeletePolicy")]
        async public Task<IActionResult> Delete(int id)
        {
            var response = await patRepo.DeleteAsync(id);
            return Ok(response);
        }
    }
}
