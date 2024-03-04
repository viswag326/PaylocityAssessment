using Api.Dtos.Employee;
using System.Reflection.Metadata.Ecma335;
using System.Linq;

namespace Api.PayrollCalculator
{
    public class ElderlyBenfitDeductionCalculator : BasePayrollDeductionCalculator
    {
        const decimal BiWeeklyElderlyDependentCost = 100;
        public override decimal CalculateDeduction(EmployeeHoursDTO employeeDTO)
        {
            int numberOfElders = employeeDTO.Dependents.Where(x => (DateTime.Now.Year - x.DateOfBirth.Year) > 50).Count();
            return numberOfElders * BiWeeklyElderlyDependentCost;
        }
    }
   
}
