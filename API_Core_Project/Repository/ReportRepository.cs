using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class ReportRepository:IDataRepositoy<ReportModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<ReportModel> collection = new CollectionResponse<ReportModel>();
        SingleObjectResponse<ReportModel> single = new SingleObjectResponse<ReportModel>();

        public ReportRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<ReportModel>> IDataRepositoy<ReportModel, int>.CreateAsync(ReportModel entity)
        {
            try
            {
                if (await IsAppoinmentIdUnique(entity.ReportID))
                {
                    var result = await ctx.Reports.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Report Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Report with similar id is present";
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

        async Task<SingleObjectResponse<ReportModel>> IDataRepositoy<ReportModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Reports.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Report based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Reports.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Report is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<ReportModel>> IDataRepositoy<ReportModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Reports.ToListAsync();
                collection.Message = "All Reports are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<ReportModel>> IDataRepositoy<ReportModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Reports.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Report based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Report Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<ReportModel>> IDataRepositoy<ReportModel, int>.UpdateAsync(int id, ReportModel entity)
        {
            try
            {
                var rec = await ctx.Reports.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Report based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                
                rec.Diagnosis = entity.Diagnosis;

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Report Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsAppoinmentIdUnique(int ReportId)
        {
            return !await ctx.Reports.AnyAsync(p => p.ReportID == ReportId);
        }
    }
}
