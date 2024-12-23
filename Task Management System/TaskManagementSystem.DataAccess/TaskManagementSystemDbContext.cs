using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Z.BulkOperations;

namespace TaskManagementSystem.DataAccess
{
    public class TaskManagementSystemDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor; 
        public TaskManagementSystemDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options) {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        private void AddAuditInfo()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AuditLog &&
                (x.State == EntityState.Added) ||
                x.State == EntityState.Modified ||
                x.State == EntityState.Deleted);

            if(entities.Any())
            {
                var timeStamp = DateTime.Now;
                var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                foreach(EntityEntry entry in entities)
                {
                    var entity = entry.Entity;
                    if(entry.State == EntityState.Added)
                    {
                        ((AuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((AuditLog)entry.Entity).CreatedAt = timeStamp;
                        ((AuditLog)entry.Entity).CreatedBy = user.ToString();
                        ((AuditLog)entry.Entity).CreatedBy = user.ToString();
                    }
                    if(entry.State == EntityState.Modified)
                    {
                        ((AuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((AuditLog)entry.Entity).UpdatedBy = user.ToString();
                    }
                    if(entry.State == EntityState.Deleted)
                    {
                        ((AuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((AuditLog)entry.Entity).UpdatedBy = user.ToString();
                        entry.State = EntityState.Modified;
                    }
                }
            }
        }

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
