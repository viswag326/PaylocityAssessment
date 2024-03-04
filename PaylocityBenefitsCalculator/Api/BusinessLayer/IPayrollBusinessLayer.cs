using Api.Dtos.Employee;

namespace Api.BusinessLayer
{
    public interface IPayrollBusinessLayer
    {
        List<EmployeePaymentDTO> ProcessPayroll(List<EmployeeHoursDTO> employeeHoursDTO);
    }
}
