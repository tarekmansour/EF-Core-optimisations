using Dapper;
using Example.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Example.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        public readonly DatabaseContext _dbContext;
        public OfficesController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPut("salaries-v1")]
        public async Task<IResult> UpdateSalaries_V1(int officeId)
        {
            var office = await _dbContext
            .Set<Office>()
            .Include(o => o.Employees)
            .FirstOrDefaultAsync(o => o.Id == officeId);

            if (office == null)
            {
                return Results.NotFound($"The office with Id '{officeId}' was not found.");
            }

            //increase office's employees salaries by 10%
            foreach (var employee in office.Employees)
            {
                employee.Salary *= 1.1m;
            }

            office.LastSalaryUpdateUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Results.NoContent();
        }

        [HttpPut("salaries-v2")]
        public async Task<IResult> UpdateSalaries_V2(int officeId)
        {
            var office = await _dbContext
            .Set<Office>()
            .FirstOrDefaultAsync(o => o.Id == officeId);

            if (office == null)
            {
                return Results.NotFound($"The office with Id '{officeId}' was not found.");
            }

            await _dbContext.Database.BeginTransactionAsync();

            //increase office's employees salaries by 10%
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Employees SET Salary = Salary * 1.1 WHERE OfficeId = {office.Id}");

            office.LastSalaryUpdateUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            await _dbContext.Database.CommitTransactionAsync();

            return Results.NoContent();
        }

        [HttpPut("salaries-vdapper")]
        public async Task<IResult> UpdateSalaries_Vdapper(int officeId)
        {
            var office = await _dbContext
            .Set<Office>()
            .FirstOrDefaultAsync(o => o.Id == officeId);

            if (office == null)
            {
                return Results.NotFound($"The office with Id '{officeId}' was not found.");
            }

            var transaction = await _dbContext.Database.BeginTransactionAsync();

            //increase office's employees salaries by 10%
            await _dbContext.Database.GetDbConnection().ExecuteAsync(
                "UPDATE Employees SET Salary = Salary * 1.1 WHERE OfficeId = @OfficeId",
                new { OfficeId = office.Id},
                transaction.GetDbTransaction());

            office.LastSalaryUpdateUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            await _dbContext.Database.CommitTransactionAsync();

            return Results.NoContent();
        }
    }
}
