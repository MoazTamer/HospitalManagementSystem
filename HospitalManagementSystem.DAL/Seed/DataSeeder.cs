using BCrypt.Net;
using HospitalManagementSystem.DAL.Data;
using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DAL.Seed
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Seed Users first
                if (!await context.Users.AnyAsync())
                {
                    var users = new List<User>
                    {
                        new User
                        {
                            Username = "admin",
                            Email = "admin@hospital.com",
                            PasswordHash = PasswordHelper.HashPassword("Admin@123"),
                            FirstName = "System",
                            LastName = "Administrator",
                            Role = "Admin",
                            IsActive = true,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "doctor1",
                            Email = "doctor1@hospital.com",
                            PasswordHash = PasswordHelper.HashPassword("Doctor@123"),
                            FirstName = "Ahmed",
                            LastName = "Hassan",
                            Role = "Doctor",
                            IsActive = true,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "receptionist",
                            Email = "reception@hospital.com",
                            PasswordHash = PasswordHelper.HashPassword("Reception@123"),
                            FirstName = "Sara",
                            LastName = "Ali",
                            Role = "Receptionist",
                            IsActive = true,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new User
                        {
                            Username = "patient1",
                            Email = "patient1@email.com",
                            PasswordHash = PasswordHelper.HashPassword("Patient@123"),
                            FirstName = "Omar",
                            LastName = "Ibrahim",
                            Role = "Patient",
                            IsActive = true,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        }
                    };

                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Users seeded successfully");
                }

                // Seed Departments
                if (!await context.Departments.AnyAsync())
                {
                    var departments = new List<Department>
                    {
                        new Department { Name = "Cardiology", Description = "Heart and cardiovascular system", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                        new Department { Name = "Neurology", Description = "Brain and nervous system", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                        new Department { Name = "Orthopedics", Description = "Bones, joints, and muscles", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                        new Department { Name = "Pediatrics", Description = "Children's health", CreatedBy = "System", CreatedDate = DateTime.UtcNow },
                        new Department { Name = "General Medicine", Description = "General health consultation", CreatedBy = "System", CreatedDate = DateTime.UtcNow }
                    };
                    await context.Departments.AddRangeAsync(departments);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Departments seeded successfully");
                }

                // Seed Doctors
                if (!await context.Doctors.AnyAsync())
                {
                    var cardiologyDept = await context.Departments.FirstAsync(d => d.Name == "Cardiology");
                    var neurologyDept = await context.Departments.FirstAsync(d => d.Name == "Neurology");
                    var orthopedicsDept = await context.Departments.FirstAsync(d => d.Name == "Orthopedics");

                    var doctors = new List<Doctor>
                    {
                        new Doctor
                        {
                            FirstName = "Ahmed",
                            LastName = "Hassan",
                            Specialization = "Cardiologist",
                            LicenseNumber = "LIC001",
                            Phone = "01012345678",
                            Email = "ahmed.hassan@hospital.com",
                            DepartmentId = cardiologyDept.Id,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new Doctor
                        {
                            FirstName = "Sara",
                            LastName = "Mohamed",
                            Specialization = "Neurologist",
                            LicenseNumber = "LIC002",
                            Phone = "01098765432",
                            Email = "sara.mohamed@hospital.com",
                            DepartmentId = neurologyDept.Id,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new Doctor
                        {
                            FirstName = "Khaled",
                            LastName = "Ali",
                            Specialization = "Orthopedic Surgeon",
                            LicenseNumber = "LIC003",
                            Phone = "01123456789",
                            Email = "khaled.ali@hospital.com",
                            DepartmentId = orthopedicsDept.Id,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        }
                    };
                    await context.Doctors.AddRangeAsync(doctors);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Doctors seeded successfully");
                }

                // Seed Patients
                if (!await context.Patients.AnyAsync())
                {
                    var patients = new List<Patient>
                    {
                        new Patient
                        {
                            FirstName = "Omar",
                            LastName = "Ibrahim",
                            DateOfBirth = new DateTime(1985, 5, 15),
                            Gender = "Male",
                            Phone = "01234567890",
                            Email = "omar.ibrahim@email.com",
                            Address = "123 Main St, Cairo",
                            BloodGroup = "O+",
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new Patient
                        {
                            FirstName = "Mona",
                            LastName = "Mahmoud",
                            DateOfBirth = new DateTime(1990, 8, 20),
                            Gender = "Female",
                            Phone = "01198765432",
                            Email = "mona.mahmoud@email.com",
                            Address = "456 Oak Ave, Giza",
                            BloodGroup = "A+",
                            Allergies = "Penicillin",
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new Patient
                        {
                            FirstName = "Youssef",
                            LastName = "Ahmed",
                            DateOfBirth = new DateTime(2010, 3, 10),
                            Gender = "Male",
                            Phone = "01087654321",
                            Email = "youssef.ahmed@email.com",
                            Address = "789 Palm St, Alexandria",
                            BloodGroup = "B+",
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        }
                    };
                    await context.Patients.AddRangeAsync(patients);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Patients seeded successfully");
                }

                // Seed Appointments
                if (!await context.Appointments.AnyAsync())
                {
                    var doctor1 = await context.Doctors.FirstAsync(d => d.LicenseNumber == "LIC001");
                    var doctor2 = await context.Doctors.FirstAsync(d => d.LicenseNumber == "LIC002");
                    var patient1 = await context.Patients.FirstAsync(p => p.Email == "omar.ibrahim@email.com");
                    var patient2 = await context.Patients.FirstAsync(p => p.Email == "mona.mahmoud@email.com");

                    var appointments = new List<Appointment>
                    {
                        new Appointment
                        {
                            PatientId = patient1.Id,
                            DoctorId = doctor1.Id,
                            AppointmentDate = DateTime.UtcNow.AddDays(2).Date.AddHours(10),
                            Status = "Scheduled",
                            Reason = "Regular checkup",
                            DurationMinutes = 30,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        },
                        new Appointment
                        {
                            PatientId = patient2.Id,
                            DoctorId = doctor2.Id,
                            AppointmentDate = DateTime.UtcNow.AddDays(3).Date.AddHours(14),
                            Status = "Scheduled",
                            Reason = "Headache consultation",
                            DurationMinutes = 45,
                            CreatedBy = "System",
                            CreatedDate = DateTime.UtcNow
                        }
                    };
                    await context.Appointments.AddRangeAsync(appointments);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Appointments seeded successfully");
                }

                Console.WriteLine("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
