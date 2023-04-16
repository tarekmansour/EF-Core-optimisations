namespace Example.Api.Entities;

public class Office
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? LastSalaryUpdateUtc { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
