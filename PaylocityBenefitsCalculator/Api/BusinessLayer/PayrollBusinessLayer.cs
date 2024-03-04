using Api.Dtos.Employee;
using Api.PayrollCalculator;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Api.BusinessLayer
{
    public class PayrollBusinessLayer : IPayrollBusinessLayer
    {
        private BasePayrollEarningsCalculator _standardEarnings;
        private BasePayrollDeductionCalculator _addOnBenefitDeductions;
        private BasePayrollDeductionCalculator _dependentsDeductions;
        private BasePayrollDeductionCalculator _elderlyBenefitDeductions;
        private IServiceProvider serviceProvider;
        private const decimal baseBenefitsCostBiWeekly = 500M;
        
        public PayrollBusinessLayer(BasePayrollEarningsCalculator standardEarning,
            IServiceProvider provider
            )
        {
            _standardEarnings = standardEarning;
            serviceProvider = provider;
            var services = serviceProvider.GetServices<BasePayrollDeductionCalculator>();
            _addOnBenefitDeductions = services.First(o => o.GetType() == typeof(AddOnBenfitDeductionCalculator));
            _dependentsDeductions = services.First(o => o.GetType() == typeof(DependentsBenfitDeductionCalculator));
            _elderlyBenefitDeductions = services.First(o => o.GetType() == typeof(ElderlyBenfitDeductionCalculator));
        }

        public List<EmployeePaymentDTO> ProcessPayroll(List<EmployeeHoursDTO> employeeHoursDTO)
        {
            List<EmployeePaymentDTO> result = new List<EmployeePaymentDTO>();
            foreach(EmployeeHoursDTO employeeHours in employeeHoursDTO)
            {
                EmployeePaymentDTO employeePaymentDTO = new EmployeePaymentDTO();
                employeePaymentDTO.EmployeeID = employeeHours.EmployeeID;
                employeePaymentDTO.TotalEarnings = _standardEarnings.CalculateEarnings(employeeHours);
                employeePaymentDTO.BaseBenifits = baseBenefitsCostBiWeekly;
                if(employeeHours.Dependents.Count > 0)
                {
                   employeePaymentDTO.DependentBenefits = _dependentsDeductions.CalculateDeduction(employeeHours);
                }
                employeePaymentDTO.AddOnBenefits = _addOnBenefitDeductions.CalculateDeduction(employeeHours);
                employeePaymentDTO.ElederyBenefits = _elderlyBenefitDeductions.CalculateDeduction(employeeHours);
                employeePaymentDTO.StateTax = _addOnBenefitDeductions.CalculateStateTax(employeePaymentDTO.TotalEarnings);
                employeePaymentDTO.FederalTax = _addOnBenefitDeductions.CalculateFederalTax(employeePaymentDTO.TotalEarnings);
                employeePaymentDTO.TotalDeductions = employeePaymentDTO.BaseBenifits + employeePaymentDTO.DependentBenefits
                                                      + employeePaymentDTO.AddOnBenefits + employeePaymentDTO.ElederyBenefits;
                employeePaymentDTO.NetPay = employeePaymentDTO.TotalEarnings - (employeePaymentDTO.TotalDeductions + employeePaymentDTO.StateTax
                                                               + employeePaymentDTO.FederalTax);

                employeePaymentDTO.NumberOfDependents = employeeHours.Dependents.Count();
                result.Add(employeePaymentDTO);
            }
            return result;
        }
    }
}
