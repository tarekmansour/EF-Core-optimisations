namespace Example.Api.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int OfficeId { get; set; }
}