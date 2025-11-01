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
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentDate)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Scheduled");

            builder.Property(a => a.Reason)
                .HasMaxLength(500);

            builder.Property(a => a.Notes)
                .HasMaxLength(1000);

            builder.Property(a => a.DurationMinutes)
                .HasDefaultValue(30);

            // Indexes
            builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate });
            builder.HasIndex(a => new { a.PatientId, a.AppointmentDate });
            builder.HasIndex(a => a.Status);

            // Relationships configured in Doctor and Patient configurations
        }
    }
}
