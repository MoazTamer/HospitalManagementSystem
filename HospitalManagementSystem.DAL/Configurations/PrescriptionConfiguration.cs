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
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.MedicationName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Dosage)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Frequency)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.DurationDays)
                .IsRequired();

            builder.Property(p => p.Instructions)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(p => p.MedicalRecordId);

            // Relationship configured in MedicalRecord configuration
        }
    }
}
