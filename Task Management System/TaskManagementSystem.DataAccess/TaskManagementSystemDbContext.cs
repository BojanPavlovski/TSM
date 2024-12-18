using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Domain;

namespace TaskManagementSystem.DataAccess
{
    public class TaskManagementSystemDbContext : DbContext
    {
        public TaskManagementSystemDbContext(DbContextOptions options) : base(options) { }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskModel>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<TaskModel>()
                .Property(x => x.Description)
                .HasMaxLength(200);

            modelBuilder.Entity<TaskModel>()
                .HasData(
                    new TaskModel { Id = 1, Name = "Create Domain Models.", Description = "Create domain model TaskModel and its properties accordingly", IsCompleted = true},
                    new TaskModel { Id = 2, Name = "Create service class library project.", Description = "Create a service class that will communicate with controllers and database", IsCompleted = false},
                    new TaskModel { Id = 3, Name = "Create DataAccess class library project.", Description = "Create a DataAccess class library project that will comunicate with a database", IsCompleted = true }
                );
        }
    }
}
