using Api.Dtos.Employee;

namespace Api.PayrollCalculator
{
    public class AddOnBenfitDeductionCalculator : BasePayrollDeductionCalculator
    {
        const int NumberOfBiWeeksInYear = 26;
        const int HoursPerWeek = 40;
        public override decimal CalculateDeduction(EmployeeHoursDTO employeeDTO)
        {
            decimal totalAnnualEarnings = 0;
            totalAnnualEarnings = employeeDTO.SalaryPerHour * NumberOfBiWeeksInYear * HoursPerWeek;

            return totalAnnualEarnings > 80000 ? (0.02m * totalAnnualEarnings) : 0;
        }
    }

}
