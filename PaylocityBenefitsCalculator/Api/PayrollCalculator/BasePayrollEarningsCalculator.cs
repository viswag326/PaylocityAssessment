using Api.Dtos.Employee;

namespace Api.PayrollCalculator
{
    public abstract class BasePayrollEarningsCalculator 
    {
        public abstract decimal CalculateEarnings(EmployeeHoursDTO employeeDTO);
    }

}
