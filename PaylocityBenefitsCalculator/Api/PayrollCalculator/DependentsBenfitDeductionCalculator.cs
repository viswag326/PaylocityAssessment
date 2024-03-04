using Api.Dtos.Employee;

namespace Api.PayrollCalculator
{
    public class DependentsBenfitDeductionCalculator : BasePayrollDeductionCalculator
    {
        const decimal BiWeeklyCostPerDependent = 300;
        public override decimal CalculateDeduction(EmployeeHoursDTO employeeDTO)
        {
            return employeeDTO.Dependents.Count() * BiWeeklyCostPerDependent;
        }

    }
}
