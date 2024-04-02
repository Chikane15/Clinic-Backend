using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class BillRepository:IDataRepositoy<BillModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<BillModel> collection = new CollectionResponse<BillModel>();
        SingleObjectResponse<BillModel> single = new SingleObjectResponse<BillModel>();

        public BillRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<BillModel>> IDataRepositoy<BillModel, int>.CreateAsync(BillModel entity)
        {
            try
            {
                if (await IsAppoinmentIdUnique(entity.BillID))
                {
                    var result = await ctx.Bills.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Bill Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Bill with similar id is present";
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

        async Task<SingleObjectResponse<BillModel>> IDataRepositoy<BillModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Bills.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Bill based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Bills.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Bill Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<BillModel>> IDataRepositoy<BillModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Bills.ToListAsync();
                collection.Message = "All Bills are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<BillModel>> IDataRepositoy<BillModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Bills.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Bill based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Bill Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<BillModel>> IDataRepositoy<BillModel, int>.UpdateAsync(int id, BillModel entity)
        {
            try
            {
                var rec = await ctx.Bills.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Bill based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.TotalCharge=entity.TotalCharge;
                rec.ConsultingCharge=entity.ConsultingCharge;
                rec.DateOfVisit=entity.DateOfVisit;
                rec.InsuranceCoverage=entity.InsuranceCoverage;
                rec.PatientID=entity.PatientID;
                

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Bill Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsAppoinmentIdUnique(int BillId)
        {
            return !await ctx.Bills.AnyAsync(p => p.BillID == BillId);
        }
    }
}
