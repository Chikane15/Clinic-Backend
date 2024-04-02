using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class PrescriptionRepository:IDataRepositoy<PrescriptionModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<PrescriptionModel> collection = new CollectionResponse<PrescriptionModel>();
        SingleObjectResponse<PrescriptionModel> single = new SingleObjectResponse<PrescriptionModel>();

        public PrescriptionRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<PrescriptionModel>> IDataRepositoy<PrescriptionModel, int>.CreateAsync(PrescriptionModel entity)
        {
            try
            {
                if (await IsAppoinmentIdUnique(entity.PriId))
                {
                    var result = await ctx.Prescriptions.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Prescription Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Prescription with similar id is present";
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

        async Task<SingleObjectResponse<PrescriptionModel>> IDataRepositoy<PrescriptionModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Prescriptions.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Prescription based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Prescriptions.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Prescription Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<PrescriptionModel>> IDataRepositoy<PrescriptionModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Prescriptions.ToListAsync();
                collection.Message = "All Prescriptions are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<PrescriptionModel>> IDataRepositoy<PrescriptionModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Prescriptions.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Prescription based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Prescription Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<PrescriptionModel>> IDataRepositoy<PrescriptionModel, int>.UpdateAsync(int id, PrescriptionModel entity)
        {
            try
            {
                var rec = await ctx.Prescriptions.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Prescription based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.Medicine=rec.Medicine;
                

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Prescription Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsAppoinmentIdUnique(int PrescriptionId)
        {
            return !await ctx.Prescriptions.AnyAsync(p => p.PriId == PrescriptionId);
        }
    }
}
