using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class AppoinmentRepository:IDataRepositoy<AppoinmentModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<AppoinmentModel> collection = new CollectionResponse<AppoinmentModel>();
        SingleObjectResponse<AppoinmentModel> single = new SingleObjectResponse<AppoinmentModel>();

        public AppoinmentRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<AppoinmentModel>> IDataRepositoy<AppoinmentModel, int>.CreateAsync(AppoinmentModel entity)
        {
            try
            {
                if (await IsAppoinmentIdUnique(entity.AppoinmentId))
                {
                    var result = await ctx.Appoinments.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Appoinment Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Appoinment with similar id is present";
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

        async Task<SingleObjectResponse<AppoinmentModel>> IDataRepositoy<AppoinmentModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Appoinments.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Appoinmet based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Appoinments.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Appoinemt Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<AppoinmentModel>> IDataRepositoy<AppoinmentModel, int>.GetAsync()
        {
            try
            {
                
                collection.Records = await ctx.Appoinments.ToListAsync();
                collection.Message = "All Appoinments are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<AppoinmentModel>> IDataRepositoy<AppoinmentModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Appoinments.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Appoinment based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Appoinment Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<AppoinmentModel>> IDataRepositoy<AppoinmentModel, int>.UpdateAsync(int id, AppoinmentModel entity)
        {
            try
            {
                var rec = await ctx.Appoinments.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Appoinment based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.PatientId = entity.PatientId;
                rec.DoctorId = entity.DoctorId;
                rec.date = entity.date;
                rec.timeSlot = entity.timeSlot;

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Appoinment Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsAppoinmentIdUnique(int AppoinmentId)
        {
            return !await ctx.Appoinments.AnyAsync(p => p.AppoinmentId == AppoinmentId);
        }
    }
}
