using Api.Dtos.Employee;

namespace Api.BusinessLayer
{
    public interface IEmployeeBusinessLayer
    {
        EmployeeValidationResultDTO ValidateEmployee(EmployeeDto emplopyeeDTO);
    }
}
