using Microsoft.EntityFrameworkCore;

namespace API_Core_Project.Models
{
    public class ClinicDbContext:DbContext
    {
        public DbSet<PatientModel> Patients { get; set; } = null!;
        public DbSet<DoctorModel> Doctors { get; set; } = null!;

        public DbSet<BillModel> Bills { get; set; } = null!;

        public DbSet<PrescriptionModel> Prescriptions { get; set; } = null!;

        public DbSet<ReportModel> Reports { get; set; } = null!;

        public DbSet<VisitModel> Visits { get; set; } = null!;

        public DbSet<AppoinmentModel> Appoinments { get; set; } = null!;

        public DbSet<DoctorImconeModel> DoctorIncomes { get; set; } = null!;
        public ClinicDbContext()
        {

        }
        // DI Resolve
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VisitModel>()
                 .HasOne<PatientModel>()
                 .WithMany()
                 .HasForeignKey(v => v.PId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitModel>()
                .HasOne<DoctorModel>()
                .WithMany()
                .HasForeignKey(v => v.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitModel>()
                .HasOne<BillModel>()
                .WithMany()
                .HasForeignKey(v => v.BillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitModel>()
               .HasOne<ReportModel>()
               .WithMany()
               .HasForeignKey(v => v.ReportID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitModel>()
               .HasOne<PrescriptionModel>()
               .WithMany()
               .HasForeignKey(v => v.PriId)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ReportModel>()
              .HasOne<PatientModel>()
              .WithMany()
              .HasForeignKey(r => r.PatientID);

            modelBuilder.Entity<ReportModel>()
             .HasOne<DoctorModel>()
             .WithMany()
             .HasForeignKey(r => r.DId);

            modelBuilder.Entity<BillModel>()
             .HasOne<PatientModel>()
             .WithMany()
             .HasForeignKey(b => b.PatientID);

            modelBuilder.Entity<DoctorImconeModel>()
             .HasOne<DoctorModel>()
             .WithMany()
             .HasForeignKey(b => b.DoctorId);

            modelBuilder.Entity<AppoinmentModel>()
             .HasOne<PatientModel>()
             .WithMany()
             .HasForeignKey(b => b.PatientId)
             .OnDelete(DeleteBehavior.Restrict);//Patient should not be delete if its Appoinment is there

            modelBuilder.Entity<AppoinmentModel>()
            .HasOne<PatientModel>()
            .WithMany()
            .HasForeignKey(b => b.DoctorId);

            modelBuilder.Entity<AppoinmentModel>()
                .Property(a => a.date)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }


    }
}
