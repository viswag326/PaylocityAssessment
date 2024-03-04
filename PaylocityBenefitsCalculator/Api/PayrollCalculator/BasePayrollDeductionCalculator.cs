using Api.Dtos.Employee;

namespace Api.PayrollCalculator
{
    public abstract class BasePayrollDeductionCalculator
    {
        const decimal StateIncomeTaxPercentage = 0.1M;
        public abstract decimal CalculateDeduction(EmployeeHoursDTO employeeDTO);

        public decimal CalculateStateTax(decimal totalEarnings)
        {
            return totalEarnings * StateIncomeTaxPercentage;
        }

        const decimal FederalIncomeTaxPercentage = 0.2M;
        public decimal CalculateFederalTax(decimal totalEarnings)
        {
            return totalEarnings * FederalIncomeTaxPercentage;
        }
    }
}
