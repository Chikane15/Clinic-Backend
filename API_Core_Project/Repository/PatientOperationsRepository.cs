using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Core_Project.Repository
{
    public class PatientOperationsRepository
    {
        ClinicDbContext ctx;

        CollectionResponse<AppoinmentModel> Appoinmentcollection = new CollectionResponse<AppoinmentModel>();
        SingleObjectResponse<PatientModel> Patientcollection = new SingleObjectResponse<PatientModel>();
        CollectionResponse<ReportModel> Reportcollection = new CollectionResponse<ReportModel>();
        CollectionResponse<BillModel> Billcollection = new CollectionResponse<BillModel>();
        CollectionResponse<PrescriptionModel> Prescriptioncollection = new CollectionResponse<PrescriptionModel>();

        CollectionResponse<VisitModel> Visitcollection = new CollectionResponse<VisitModel>();
        SingleObjectResponse<DoctorImconeModel> IncomeDetailssingle = new SingleObjectResponse<DoctorImconeModel>();

        public PatientOperationsRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<CollectionResponse<AppoinmentModel>> GetAsyncAppoinment(int id)
        {

            try
            {
                var result = await ctx.Appoinments.Where(p => p.PatientId == id).ToListAsync();

                if (!result.Any())
                {

                    Appoinmentcollection.Message = "No appoinment !!";
                    Appoinmentcollection.StatusCode = 500;
                }
                else
                {
                    Appoinmentcollection.Records = result;
                    Appoinmentcollection.Message = "All appoinments are read successfully";
                    Appoinmentcollection.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                Appoinmentcollection.Message = ex.Message;
                Appoinmentcollection.StatusCode = 500;
            }
            return Appoinmentcollection;
        }

        public async Task<SingleObjectResponse<PatientModel>> GetAsyncPatientDetails(int id)
        {

            try
            {
                var result = await ctx.Patients.Where(p=>p.PatientID==id).FirstOrDefaultAsync();

             
                    Patientcollection.Record = result;
                    Patientcollection.Message = "Patient Information";
                    Patientcollection.StatusCode = 200;
                
            }
            catch (Exception ex)
            {
                Patientcollection.Message = ex.Message;
                Patientcollection.StatusCode = 500;
            }
            return Patientcollection;
        }

        public async Task<CollectionResponse<ReportModel>> GetAsyncReports(int id)
        {

            try
            {
                var result = await ctx.Reports.Where(p => p.PatientID == id).ToListAsync();

                if (!result.Any())
                {

                    Reportcollection.Message = "No reports for you !!";
                    Reportcollection.StatusCode = 500;
                }
                else
                {
                    Reportcollection.Records = result;
                    Reportcollection.Message = "All appoinments are read successfully";
                    Reportcollection.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                Reportcollection.Message = ex.Message;
                Reportcollection.StatusCode = 500;
            }
            return Reportcollection;
        }

        public async Task<CollectionResponse<BillModel>> GetAsyncBills(int id)
        {

            try
            {
                var result = await ctx.Bills.Where(p => p.PatientID == id).ToListAsync();

                if (!result.Any())
                {

                    Billcollection.Message = "No bills for you !!";
                    Billcollection.StatusCode = 500;
                }
                else
                {
                    Billcollection.Records = result;
                    Billcollection.Message = "All bills are read successfully";
                    Billcollection.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                Billcollection.Message = ex.Message;
                Billcollection.StatusCode = 500;
            }
            return Billcollection;
        }

        //We have to add PatientId in Prescription Model to do so
       public async Task<CollectionResponse<PrescriptionModel>> GetAsyncPrescription(int id)
        {

            try
            {
                var result = await ctx.Prescriptions.Where(p => p.PatientId == id).ToListAsync();

                if (!result.Any())
                {

                    Prescriptioncollection.Message = "No prescription for you !!";
                    Prescriptioncollection.StatusCode = 500;
                }
                else
                {
                    Prescriptioncollection.Records = result;
                    Prescriptioncollection.Message = "All prescriptions are read successfully";
                    Prescriptioncollection.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                Prescriptioncollection.Message = ex.Message;
                Prescriptioncollection.StatusCode = 500;
            }
            return Prescriptioncollection;
        }
    }
}
