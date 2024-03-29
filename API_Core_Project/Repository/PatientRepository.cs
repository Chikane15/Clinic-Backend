using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class PatientRepository : IDataRepositoy<PatientModel, int>
    {
        ClinicDbContext ctx;

        CollectionResponse<PatientModel> collection = new CollectionResponse<PatientModel>();
        SingleObjectResponse<PatientModel> single = new SingleObjectResponse<PatientModel>();

        public PatientRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<PatientModel>> IDataRepositoy<PatientModel, int>.CreateAsync(PatientModel entity)
        {
            try
            {
                if (await IsPatientIdUnique(entity.PatientID))
                {
                    var result = await ctx.Patients.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Patient Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Patient with similar id is present";
                    single.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                single.Message = ex.Message;
                single.StatusCode = 500;
            }
            return single;
        }

        async Task<SingleObjectResponse<PatientModel>> IDataRepositoy<PatientModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Patients.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Patient based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Patients.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Patient Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<PatientModel>> IDataRepositoy<PatientModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Patients.ToListAsync();
                collection.Message = "All Patients are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<PatientModel>> IDataRepositoy<PatientModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Patients.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Patient based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Patient Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<PatientModel>> IDataRepositoy<PatientModel, int>.UpdateAsync(int id, PatientModel entity)
        {
            try
            {
                var rec = await ctx.Patients.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Patient based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                
                rec.FirstName = entity.FirstName;
                rec.LastName = entity.LastName;
                rec.Email = entity.Email;
                rec.Address = entity.Address;
                rec.BloodType = entity.BloodType;
                rec.EmergencyContact = entity.EmergencyContact;
                rec.BloodType=entity.BloodType;
                rec.Contact=entity.Contact;
                rec.DOB = entity.DOB;
                rec.Insurance = entity.Insurance;
                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Patient Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsPatientIdUnique(int patientId)
        {
            return !await ctx.Patients.AnyAsync(p=>p.PatientID==patientId);
        }
    }
}
