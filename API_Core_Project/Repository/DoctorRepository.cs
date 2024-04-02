using API_Core_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace API_Core_Project.Repository
{
    public class DoctorRepository : IDataRepositoy<DoctorModel, int>
    {
        ClinicDbContext ctx;

        CollectionResponse<DoctorModel> collection = new CollectionResponse<DoctorModel>();
        SingleObjectResponse<DoctorModel> single = new SingleObjectResponse<DoctorModel>();

        public DoctorRepository(ClinicDbContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<SingleObjectResponse<DoctorModel>> IDataRepositoy<DoctorModel, int>.CreateAsync(DoctorModel entity)
        {
            try
            {
                if (await IsDocotorIdUnique(entity.DoctorID))
                {
                    var result = await ctx.Doctors.AddAsync(entity);
                    await ctx.SaveChangesAsync();
                    single.Record = result.Entity;
                    single.Message = "New Doctor Record is added successfully";
                    single.StatusCode = 200;
                }
                else
                {
                    single.Message = "Doctor with similar id is present";
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

        async Task<SingleObjectResponse<DoctorModel>> IDataRepositoy<DoctorModel, int>.DeleteAsync(int id)
        {
            try
            {
                var recToDelete = await ctx.Doctors.FindAsync(id);
                if (recToDelete == null)
                {
                    single.Message = $"Doctor based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                ctx.Doctors.Remove(recToDelete);
                await ctx.SaveChangesAsync();

                single.Message = "Doctor Record is deleted successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<CollectionResponse<DoctorModel>> IDataRepositoy<DoctorModel, int>.GetAsync()
        {
            try
            {
                collection.Records = await ctx.Doctors.ToListAsync();
                collection.Message = "All Doctors are read successfully";
                collection.StatusCode = 200;
            }
            catch (Exception ex)
            {
                collection.Message = ex.Message;
                collection.StatusCode = 500;
            }
            return collection;
        }

        async Task<SingleObjectResponse<DoctorModel>> IDataRepositoy<DoctorModel, int>.GetAsync(int id)
        {
            try
            {
                var rec = await ctx.Doctors.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Doctor based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                single.Record = rec;
                single.Message = "Doctor Record is found successfully";
                single.StatusCode = 200;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        async Task<SingleObjectResponse<DoctorModel>> IDataRepositoy<DoctorModel, int>.UpdateAsync(int id, DoctorModel entity)
        {
            try
            {
                var rec = await ctx.Doctors.FindAsync(id);
                if (rec == null)
                {
                    single.Message = $"Doctor based on Id={id} is not found";
                    single.StatusCode = 500;
                    // Throwing the Custom Message 
                    throw new Exception(JsonSerializer.Serialize(single));
                }

                rec.FirstName = entity.FirstName;
                rec.LastName = entity.LastName;
                rec.Speciality = entity.Speciality;
               
                await ctx.SaveChangesAsync();
                single.Record = rec;
                single.Message = "Doctor Record is updated successfully";
                single.StatusCode = 200;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return single;
        }

        private async Task<bool> IsDocotorIdUnique(int doctorId)
        {
            return !await ctx.Doctors.AnyAsync(p => p.DoctorID == doctorId);
        }
    }
}
