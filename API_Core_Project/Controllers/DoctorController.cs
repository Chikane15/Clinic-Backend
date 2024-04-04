using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        IDataRepositoy<PatientModel, int> patRepo;
        IDataRepositoy<DoctorModel, int> docRepo;
        IDataRepositoy<AppoinmentModel, int> appoinmentRepo;
        IDataRepositoy<ReportModel, int> reportRepo;
        IDataRepositoy<PrescriptionModel, int> prescriptionRepo;
        DoctorOperationRepository doctorOperationRepository;

        public DoctorController(IDataRepositoy<PatientModel, int> patRepo, IDataRepositoy<DoctorModel, int> docRepo,IDataRepositoy<AppoinmentModel,int> appRepo,IDataRepositoy<ReportModel,int> reportRepo, IDataRepositoy<PrescriptionModel, int> prescriptionRepo, DoctorOperationRepository dop)
        {
            this.patRepo = patRepo;
            this.docRepo = docRepo;
            this.appoinmentRepo = appRepo;
            this.reportRepo = reportRepo;
            this.prescriptionRepo = prescriptionRepo;
            this.doctorOperationRepository = dop;
        }

        [HttpGet]
        [Authorize(Policy = "DoctorPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetReports()
        {
            var response = await reportRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetReport")]
        [Authorize(Policy = "DoctorPolicy")]

        async public Task<IActionResult> GetReport(int id)
        {
            var response = await reportRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostReport")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> PostReport(ReportModel app)
        {
            var response = await reportRepo.CreateAsync(app);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [ActionName("PutReport")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> PutReport(int id, ReportModel app)
        {
            var response = await reportRepo.UpdateAsync(id, app);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ActionName("DeleteReport")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> DeleteReport(int id)
        {
            var response = await reportRepo.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetPres")]
        [Authorize(Policy = "DoctorPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetPres()
        {
            var response = await prescriptionRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetPre")]
        [Authorize(Policy = "DoctorPolicy")]

        async public Task<IActionResult> Getpre(int id)
        {
            var response = await prescriptionRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostPre")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> PostPre(PrescriptionModel app)
        {
            var response = await prescriptionRepo.CreateAsync(app);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [ActionName("PutPre")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> PutPre(int id, PrescriptionModel app)
        {
            var response = await prescriptionRepo.UpdateAsync(id, app);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ActionName("DeletePre")]
        [Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> DeletePre(int id)
        {
            var response = await prescriptionRepo.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetAppoinment")]
        [Authorize(Policy = "DoctorPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetAppoinments(int id)
        {
            var response = await doctorOperationRepository.GetAsyncAppoinment(id) ;
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetVisits")]
        [Authorize(Policy = "DoctorPolicy")]

        async public Task<IActionResult> GetVisits(int id)
        {
            var response = await doctorOperationRepository.GetAsyncVisits(id);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetIncomeDetials")]
        [Authorize(Policy = "DoctorPolicy")]

        async public Task<IActionResult> GetIncomeDetails(int id)
        {
            var response = await doctorOperationRepository.GetAsyncIcomeDetails(id);
            return Ok(response);
        }
    }
}
