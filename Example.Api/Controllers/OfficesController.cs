using Example.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPut("salaries")]
        public async Task<IResult> UpdateSalaries(int officeId)
        {
            var office = await _dbContext
            .Set<Office>()
            .Include(o => o.Employees)
            .FirstOrDefaultAsync(o => o.Id == officeId);

            if (office == null)
            {
                return Results.NotFound($"The office with Id '{officeId}' was not found.");
            }

            foreach (var employee in office.Employees)
            {
                employee.Salary *= 1.1m; //increase office's employees salaries by 10%
            }

            office.LastSalaryUpdateUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Results.NoContent();
        }
    }
}
