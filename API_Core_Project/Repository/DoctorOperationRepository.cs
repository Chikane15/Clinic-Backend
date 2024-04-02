using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;

namespace API_Core_Project.Repository
{
    public class DoctorOperationRepository 
    {

        ClinicDbContext ctx;

        CollectionResponse<AppoinmentModel> Appoinmentcollection = new CollectionResponse<AppoinmentModel>();
       

        CollectionResponse<VisitModel> Visitcollection = new CollectionResponse<VisitModel>();
        SingleObjectResponse<DoctorImconeModel> IncomeDetailssingle = new SingleObjectResponse<DoctorImconeModel>();

        public DoctorOperationRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
    

        public async Task<CollectionResponse<AppoinmentModel>> GetAsyncAppoinment(int id)
        {

            try
            {
                var result = await ctx.Appoinments.Where(p => p.DoctorId == id).ToListAsync();

                if (!result.Any())
                {

                    Appoinmentcollection.Message = "No appoinment today !!";
                    Appoinmentcollection.StatusCode = 500;
                }
                else
                {
                    Appoinmentcollection.Records = await ctx.Appoinments.Where(p => p.DoctorId == id).ToListAsync();
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

        public async Task<CollectionResponse<VisitModel>> GetAsyncVisits(int id)
        {
            try
            {
                var result = await ctx.Visits.Where(p => p.DoctorId == id).ToListAsync();

                if (!result.Any())
                {

                    Visitcollection.Message = "No Visits today !!";
                    Visitcollection.StatusCode = 500;
                }
                else
                {
                    Visitcollection.Records = await ctx.Visits.Where(p => p.DoctorId == id).ToListAsync();
                    Visitcollection.Message = "All visits are read successfully";
                    Visitcollection.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                Visitcollection.Message = ex.Message;
                Visitcollection.StatusCode = 500;
            }
            return Visitcollection;
        }

        public async Task<SingleObjectResponse<DoctorImconeModel>> GetAsyncIcomeDetails(int id)
        {
            try
            {
                var rec = await ctx.DoctorIncomes.FindAsync(id);
                if (rec == null)
                {
                    IncomeDetailssingle.Message = $"Income Details for doctor based on Id={id} is not found";
                    IncomeDetailssingle.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(IncomeDetailssingle));
                }

                IncomeDetailssingle.Record = rec;
                IncomeDetailssingle.Message = "Income Detail for doctor is found successfully";
                IncomeDetailssingle.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IncomeDetailssingle;
        }
    }
}
