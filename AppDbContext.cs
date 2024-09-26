using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vueapi.Model;

namespace Vueapi
{
    public class AppDbContext : IdentityDbContext<User> 

    {

        public AppDbContext() { }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Device> Devices { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasMany(e => e. Devices).WithOne(d => d.employee).HasForeignKey(d => d.AssignedTo);
           // modelBuilder.Entity<Employee>().HasMany(e => e.Devices).WithMany(d => d.Assignments).HasForeignKey(d => d);

            modelBuilder.Entity<Employee>().HasMany(e => e.Assignments).WithOne(d => d.employee).HasForeignKey(d => d.EmployeeId);
            modelBuilder.Entity<Device>().HasMany(e => e.Assignments).WithOne(d => d.device).HasForeignKey(d => d.DeviceId);

        }


    }
}
