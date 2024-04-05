using API_Core_Project.Models;
using API_Core_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Core_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IDataRepositoy<PatientModel, int> patRepo;
        IDataRepositoy<DoctorModel, int> docRepo;
        IDataRepositoy<AppoinmentModel, int> appoinmentRepo;
        IDataRepositoy<DoctorImconeModel, int> docImconeRepo;
        IDataRepositoy<VisitModel, int> visitRepo;
        IDataRepositoy<BillModel, int> billRepo;

        public AdminController(IDataRepositoy<PatientModel, int> patRepo, IDataRepositoy<DoctorModel, int> docRepo, IDataRepositoy<AppoinmentModel, int> appoinmentRepo, IDataRepositoy<DoctorImconeModel, int> docImconeRepo, IDataRepositoy<VisitModel, int> visitRepo, IDataRepositoy<BillModel, int> billRepo)
        {
            this.patRepo = patRepo;
            this.docRepo = docRepo;
            this.appoinmentRepo = appoinmentRepo;
            this.docImconeRepo = docImconeRepo;
            this.visitRepo = visitRepo;
            this.billRepo = billRepo;
        }

        [HttpGet]
        [ActionName("GetAppoinments")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> Get()
        {
            var response = await appoinmentRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetAppoinment")]
/*        [Authorize(Policy = "AdminPolicy")]
*/
        async public Task<IActionResult> Get(int id)
        {
            var response = await appoinmentRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostAppoinment")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> Post(AppoinmentModel app)
        {
            var response = await appoinmentRepo.CreateAsync(app);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [ActionName("PutAppoinment")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> Put(int id, AppoinmentModel app)
        {
            var response = await appoinmentRepo.UpdateAsync(id, app);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ActionName("DeleteAppoinment")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> Delete(int id)
        {
            var response = await appoinmentRepo.DeleteAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetDoctors")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetDocs()
        {
            var response = await docRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetDoctor")]
/*        [Authorize(Policy = "AdminPolicy")]
*/
        async public Task<IActionResult> GetDoc(int id)
        {
            var response = await docRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostDoctors")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> PostDoc(DoctorModel doctor)
        {
            var response = await docRepo.CreateAsync(doctor);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [ActionName("PutDoctors")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> PutDoc(int id, DoctorModel doctor)
        {
            var response = await docRepo.UpdateAsync(id, doctor);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ActionName("DeleteDoctors")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> DeleteDoc(int id)
        {
            var response = await docRepo.DeleteAsync(id);
            return Ok(response);
        }
        [HttpGet]
        [ActionName("GetPatients")]
        //[Authorize(Policy = "AdminPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetPats()
        {
            var response = await patRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetPatient")]
/*        [Authorize(Policy = "AdminPolicy")]
*/
        async public Task<IActionResult> GetPat(int id)
        {
            var response = await patRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostPatient")]
        //[Authorize(Policy = "AdminPolicy")]
        async public Task<IActionResult> PostPat(PatientModel patient)
        {
            var response = await patRepo.CreateAsync(patient);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [ActionName("PutPatient")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> PutPat(int id, PatientModel patient)
        {
            var response = await patRepo.UpdateAsync(id, patient);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        [ActionName("DeletePatient")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> DeletePat(int id)
        {
            var response = await patRepo.DeleteAsync(id);
            return Ok(response);
        }


        [HttpGet]
        [ActionName("GetDIs")]
        [Authorize(Policy = "AdminPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetDIs()
        {
            var response = await docImconeRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetDI")]
/*        [Authorize(Policy = "AdminPolicy")]
*/
        async public Task<IActionResult> GetDI(int id)
        {
            var response = await docImconeRepo.GetAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [ActionName("PostDI")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> PostDI(DoctorImconeModel di)
        {
            var response = await docImconeRepo.CreateAsync(di);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ActionName("PutDI")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        async public Task<IActionResult> PutDI(int id, DoctorImconeModel di)
        {
            var response = await docImconeRepo.UpdateAsync(id, di);
            return Ok(response);
        }


        [HttpGet]
        [ActionName("GetVisits")]
/*        [Authorize(Policy = "AdminPolicy")]
*/        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetVisits()
        {
            var response = await visitRepo.GetAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ActionName("GetVisit")]
/*        [Authorize(Policy = "AdminPolicy")]
*/
        async public Task<IActionResult> GetVisit(int id)
        {
            var response = await visitRepo.GetAsync(id);
            return Ok(response);
        }

        [HttpPost]
        [ActionName("PostBill")]
        //[Authorize(Policy = "DoctorPolicy")]
        async public Task<IActionResult> PostBill(BillModel app)
        {
            var response = await billRepo.CreateAsync(app);
            return Ok(response);
        }

        [HttpGet]
        [ActionName("GetBills")]
        // [Authorize(Policy = "DoctorPolicy")]
        //[Authorize(Roles = "Manager,Clerk,Operator")]

        async public Task<IActionResult> GetBills()
        {
            var response = await billRepo.GetAsync();
            return Ok(response);
        }
    }
}
