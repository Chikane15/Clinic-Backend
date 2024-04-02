using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   // [Authorize(Policy = "PatientPolicy")]
    public class PatientController : ControllerBase
    {
       
        PatientOperationsRepository patOperationRepo;

        public PatientController(PatientOperationsRepository patRepoRepository)
        {
          
            this.patOperationRepo = patRepoRepository;
        }


        [HttpGet("{id}")]
        [ActionName("GetAppoinments")]
        

        async public Task<IActionResult> GetApp(int id)
        {
            var response = await patOperationRepo.GetAsyncAppoinment(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetPatient")]
        async public Task<IActionResult> GetPatient(int id)
        {
            var response = await patOperationRepo.GetAsyncPatientDetails(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetReports")]
        async public Task<IActionResult> GetReports(int id)
        {
            var response = await patOperationRepo.GetAsyncReports(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetBills")]
        async public Task<IActionResult> GetBills(int id)
        {
            var response = await patOperationRepo.GetAsyncBills(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetPrescription")]


        async public Task<IActionResult> GetPres(int id)
        {
            var response = await patOperationRepo.GetAsyncPrescription(id);
            return Ok(response);
        }
    }
}
