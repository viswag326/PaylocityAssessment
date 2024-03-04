using Api.Dtos.Employee;

namespace Api.Repository
{
    public interface IPayrollRepository
    {
        void CreatePayPeriodSchedule(int year);

        string AddEmployeeWorkingHours(List<EmployeeHoursDTO> employeeHours);

        List<EmployeeHoursDTO> GetEmployeeDetailsForProcessingPayroll(DateTime payPeriodStartDate, DateTime payPeriodEndDate);

        void UpdatePayrollDataInDB(List<EmployeePaymentDTO> employessHours, DateTime payPeriodStartDate, DateTime payPeriodEndDate);
    }
}
