using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Models
{
    public partial class PaylocityBenefitsContext : DbContext
    {
        public PaylocityBenefitsContext()
        {
        }

        public PaylocityBenefitsContext(DbContextOptions<PaylocityBenefitsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dependent> Dependents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePayment> EmployeePayments { get; set; }
        public virtual DbSet<PayPeriodSchedule> PayPeriodSchedules { get; set; }
        public virtual DbSet<RelationShip> RelationShips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PaylocityBenefits;Integrated Security=True;");

                var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");

                IConfigurationRoot configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("Dev");

                optionsBuilder.UseSqlServer(connectionString);


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dependent>(entity =>
            {
                entity.ToTable("Dependent");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RelationShipId).HasColumnName("RelationShipID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Dependents)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dependent_Employee");

                entity.HasOne(d => d.RelationShip)
                    .WithMany(p => p.Dependents)
                    .HasForeignKey(d => d.RelationShipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dependent_Dependent");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SalaryPerHour).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<EmployeePayment>(entity =>
            {
                entity.ToTable("EmployeePayment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddOnBenefits).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BaseBenefits).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DependentBenefits).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ElderlyBenefits).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FederalTax).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NetPay).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PayPeriodScheduleId).HasColumnName("PayPeriodScheduleID");

                entity.Property(e => e.StateTax).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalDeductions).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TotalEarnings).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeePayments)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePayment_Employee");

                entity.HasOne(d => d.PayPeriodSchedule)
                    .WithMany(p => p.EmployeePayments)
                    .HasForeignKey(d => d.PayPeriodScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeePayment_PayPeriodSchedule");
            });

            modelBuilder.Entity<PayPeriodSchedule>(entity =>
            {
                entity.ToTable("PayPeriodSchedule");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<RelationShip>(entity =>
            {
                entity.ToTable("RelationShip");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
