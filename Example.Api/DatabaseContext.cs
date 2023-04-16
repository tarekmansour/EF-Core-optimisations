using Example.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.Api;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Office>(builder =>
        {
            builder.ToTable("Offices");

            builder.Property(p => p.Name).HasMaxLength(150);

            builder.HasMany(office => office.Employees)
            .WithOne()
            .HasForeignKey(employee => employee.OfficeId)
            .IsRequired();

            builder.HasData(new Office
            {
                Id = 1,
                Name = "Leasing Cars Office",
            });
        });

        modelBuilder.Entity<Employee>(builder =>
        {
            builder.ToTable("Employees");

            builder.Property(p => p.FullName).HasMaxLength(200);

            var employees = Enumerable
                .Range(1, 1000)
                .Select(id => new Employee
                {
                    Id = id,
                    FullName = $"Employee #{id}",
                    Salary = 100.0m,
                    OfficeId = 1
                })
                .ToList();

            builder.HasData(employees);
        });
    }
}
