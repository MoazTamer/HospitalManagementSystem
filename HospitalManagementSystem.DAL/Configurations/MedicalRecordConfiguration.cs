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
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.HasKey(mr => mr.Id);

            builder.Property(mr => mr.VisitDate)
                .IsRequired();

            builder.Property(mr => mr.Diagnosis)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(mr => mr.Symptoms)
                .HasMaxLength(2000);

            builder.Property(mr => mr.Treatment)
                .HasMaxLength(2000);

            builder.Property(mr => mr.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(mr => new { mr.PatientId, mr.VisitDate });
            builder.HasIndex(mr => mr.DoctorId);

            // Relationships
            builder.HasMany(mr => mr.Prescriptions)
                .WithOne(p => p.MedicalRecord)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
