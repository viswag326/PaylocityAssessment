using Api.Dtos.Dependent;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Dtos.Employee;

public class EmployeeDto
{
    
    public int EmployeeID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal SalaryPerHour { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<DependentDto> Dependents { get; set; } = new List<DependentDto>();
}
