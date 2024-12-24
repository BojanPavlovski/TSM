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

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        //{

        //    AddAuditInfo();

        //    return (await base.SaveChangesAsync(true, cancellationToken));
        //}
        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        private void AddAuditInfo()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditLog &&
                (x.State == EntityState.Added) ||
                x.State == EntityState.Modified ||
                x.State == EntityState.Deleted).ToList();

            
            
                var timeStamp = DateTime.Now;
                var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                foreach(EntityEntry entry in entities)
                {
                    var entity = entry.Entity;
                    if(entry.State == EntityState.Added)
                    {
                        ((IAuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((IAuditLog)entry.Entity).CreatedAt = timeStamp;
                        ((IAuditLog)entry.Entity).CreatedBy = user.ToString();
                        ((IAuditLog)entry.Entity).UpdatedBy = user.ToString();
                    }
                    if(entry.State == EntityState.Modified)
                    {
                        ((IAuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((IAuditLog)entry.Entity).UpdatedBy = user.ToString();
                    }
                    if(entry.State == EntityState.Deleted)
                    {
                        ((IAuditLog)entry.Entity).UpdatedAt = timeStamp;
                        ((IAuditLog)entry.Entity).UpdatedBy = user.ToString();
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
