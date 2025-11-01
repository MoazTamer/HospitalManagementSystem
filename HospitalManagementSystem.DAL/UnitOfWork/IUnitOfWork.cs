using HospitalManagementSystem.DAL.Repositories.Interfaces;
using HospitalManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<Doctor> Doctors { get; }
        IGenericRepository<Patient> Patients { get; }
        IGenericRepository<Appointment> Appointments { get; }
        IGenericRepository<MedicalRecord> MedicalRecords { get; }
        IGenericRepository<Prescription> Prescriptions { get; }
        IGenericRepository<Billing> Billings { get; }
        IGenericRepository<AuditLog> AuditLogs { get; }
        IGenericRepository<User> Users { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
