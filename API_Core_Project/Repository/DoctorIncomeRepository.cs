using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class DoctorIncomeRepository:IDataRepositoy<DoctorImconeModel,int>
    {
        ClinicDbContext ctx;

        CollectionResponse<DoctorImconeModel> collection = new CollectionResponse<DoctorImconeModel>();
        SingleObjectResponse<DoctorImconeModel> single = new SingleObjectResponse<DoctorImconeModel>();

        public DoctorIncomeRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<DoctorImconeModel>> IDataRepositoy<DoctorImconeModel, int>.CreateAsync(DoctorImconeModel entity)
        {
            try
            {
                if (await IsIncomeIdUnique(entity.DoctorId))
                {
                    var result = await ctx.DoctorIncomes.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Income Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Income record with similar id is present";
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

        async Task<SingleObjectResponse<DoctorImconeModel>> IDataRepositoy<DoctorImconeModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.DoctorIncomes.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Appoinmet based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.DoctorIncomes.Remove(recToDelete);
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

        async Task<CollectionResponse<DoctorImconeModel>> IDataRepositoy<DoctorImconeModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.DoctorIncomes.ToListAsync();
                collection.Message = "All DoctorIncomes are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<DoctorImconeModel>> IDataRepositoy<DoctorImconeModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.DoctorIncomes.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Income for doctor based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Income Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<DoctorImconeModel>> IDataRepositoy<DoctorImconeModel, int>.UpdateAsync(int id, DoctorImconeModel entity)
        {
            try
            {
                var rec = await ctx.DoctorIncomes.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Income record for doctor based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.Salary = entity.Salary;
                

                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Income Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsIncomeIdUnique(int doctor)
        {
            return !await ctx.DoctorIncomes.AnyAsync(p => p.DoctorId == doctor);
        }
    }
}
