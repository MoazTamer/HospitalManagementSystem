using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DAL.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Gender)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Phone)
                .HasMaxLength(15);

            builder.Property(p => p.Email)
                .HasMaxLength(100);

            builder.Property(p => p.Address)
                .HasMaxLength(500);

            builder.Property(p => p.BloodGroup)
                .HasMaxLength(20);

            builder.Property(p => p.Allergies)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(p => p.Email);
            builder.HasIndex(p => p.Phone);

            // Relationships
            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.MedicalRecords)
                .WithOne(mr => mr.Patient)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Billings)
                .WithOne(b => b.Patient)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
