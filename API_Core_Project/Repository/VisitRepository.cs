using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class VisitRepository:IDataRepositoy<VisitModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<VisitModel> collection = new CollectionResponse<VisitModel>();
        SingleObjectResponse<VisitModel> single = new SingleObjectResponse<VisitModel>();

        public VisitRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<VisitModel>> IDataRepositoy<VisitModel, int>.CreateAsync(VisitModel entity)
        {
            try
            {
                if (await IsAppoinmentIdUnique(entity.VId))
                {
                    var result = await ctx.Visits.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Visit Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Visit with similar id is present";
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

        async Task<SingleObjectResponse<VisitModel>> IDataRepositoy<VisitModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Visits.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Visit based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Visits.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Visit Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<VisitModel>> IDataRepositoy<VisitModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Visits.ToListAsync();
                collection.Message = "All Visits are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<VisitModel>> IDataRepositoy<VisitModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Visits.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Visit based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Visit Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<VisitModel>> IDataRepositoy<VisitModel, int>.UpdateAsync(int id, VisitModel entity)
        {
            try
            {
                var rec = await ctx.Visits.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Visit based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

               rec.ReportID = entity.ReportID;
            rec.PriId = entity.PriId;
                rec.PId = entity.PId;
                rec.BillId = entity.BillId;
                rec.DateofVisit = entity.DateofVisit;
                rec.DoctorId = entity.DoctorId;
                rec.TimeSlot = entity.TimeSlot;
                

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Visit Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsAppoinmentIdUnique(int VId)
        {
            return !await ctx.Visits.AnyAsync(p => p.VId == VId);
        }
    }
}
