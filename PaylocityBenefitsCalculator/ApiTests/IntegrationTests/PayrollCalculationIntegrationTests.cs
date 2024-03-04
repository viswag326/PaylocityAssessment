using Api.BusinessLayer;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.PayrollCalculator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests
{
    public class PayrollBusinessLayerTests
    {
        [Fact]
        public void ProcessPayroll_CalculatesCorrectNetPayWithoutDependents()
        {
            var serviceProvider = new ServiceCollection()
                 .AddTransient<BasePayrollDeductionCalculator, AddOnBenfitDeductionCalculator>()
                 .AddTransient<BasePayrollDeductionCalculator, DependentsBenfitDeductionCalculator>()
                 .AddTransient<BasePayrollDeductionCalculator, ElderlyBenfitDeductionCalculator>()
                 .BuildServiceProvider();

            var standardEarningsCalculator = new MockStandardEarningCalculator();
            var payrollBusinessLayer = new PayrollBusinessLayer(standardEarningsCalculator, serviceProvider);

            var employeeHoursDTO = new List<EmployeeHoursDTO>
            {
                 new EmployeeHoursDTO(){ EmployeeID = 1, FirstName = "Ann", LastName = "J",
                     DateOfBirth = DateTime.Now.AddYears(-30), RegularHours = 80, SalaryPerHour = 100, Dependents = new List<DependentDto>()
                                         }
            };

            var result = payrollBusinessLayer.ProcessPayroll(employeeHoursDTO);
            Assert.True(result.Count == 1);
            Assert.Equal(500, result[0].BaseBenifits);
            Assert.Equal(0, result[0].DependentBenefits);
            Assert.Equal(2080, result[0].AddOnBenefits);
            Assert.Equal(1600, result[0].FederalTax);
            Assert.Equal(800, result[0].StateTax);
            Assert.Equal(8000, result[0].TotalEarnings);
            Assert.Equal(2580, result[0].TotalDeductions);
            Assert.Equal(3020, result[0].NetPay);
        }

        [Fact]
        public void ProcessPayroll_CalculatesCorrectNetPayWithDependents()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<BasePayrollDeductionCalculator, AddOnBenfitDeductionCalculator>()
                .AddTransient<BasePayrollDeductionCalculator, DependentsBenfitDeductionCalculator>()
                .AddTransient<BasePayrollDeductionCalculator, ElderlyBenfitDeductionCalculator>()
                .BuildServiceProvider();

            var standardEarningsCalculator = new MockStandardEarningCalculator();
            var payrollBusinessLayer = new PayrollBusinessLayer(standardEarningsCalculator, serviceProvider);

            var employeeHoursDTO = new List<EmployeeHoursDTO>
            {
                 new EmployeeHoursDTO(){ EmployeeID = 1, FirstName = "Ann", LastName = "J", 
                     DateOfBirth = DateTime.Now.AddYears(-30), RegularHours = 80, SalaryPerHour = 100, Dependents = new List<DependentDto>()
                     {
                          new DependentDto(){ FirstName = "Child ", LastName = "1" , DateOfBirth = DateTime.Now.AddYears(-6), Relationship = Api.Models.Relationship.Child },
                          new DependentDto(){ FirstName = "Spouse ", LastName = "LN" , DateOfBirth = DateTime.Now.AddYears(25), Relationship = Api.Models.Relationship.Spouse },
                     }
                     }
            };

            var result = payrollBusinessLayer.ProcessPayroll(employeeHoursDTO);
            Assert.True(result.Count == 1);
            Assert.Equal(500, result[0].BaseBenifits);
            Assert.Equal(600, result[0].DependentBenefits);
            Assert.Equal(2080, result[0].AddOnBenefits);
            Assert.Equal(1600, result[0].FederalTax);
            Assert.Equal(800, result[0].StateTax);
            Assert.Equal(8000, result[0].TotalEarnings);
            Assert.Equal(3180, result[0].TotalDeductions);
            Assert.Equal(2420, result[0].NetPay);
        }

        [Fact]
        public void CalculateDeduction_AddObBenfitDeduction()
        {
            var calculator = new AddOnBenfitDeductionCalculator();
            var employeeDTO = new EmployeeHoursDTO
            {
                SalaryPerHour = 80,
                RegularHours = 40
            };
            var result = calculator.CalculateDeduction(employeeDTO);
            Assert.Equal(1664, result);
        }

        [Fact]
        public void CalculateDeduction_ReturnsCorrectDeduction()
        {
           
            var calculator = new DependentsBenfitDeductionCalculator();
            var employeeDTO = new EmployeeHoursDTO
            {
                Dependents = new List<DependentDto>
                {
                     new DependentDto(){ FirstName = "Child ", LastName = "1" , DateOfBirth = DateTime.Now.AddYears(-6), Relationship = Api.Models.Relationship.Child },
                     new DependentDto(){ FirstName = "Spouse ", LastName = "LN" , DateOfBirth = DateTime.Now.AddYears(25), Relationship = Api.Models.Relationship.Spouse },
                }
            };

       
            var result = calculator.CalculateDeduction(employeeDTO);
            Assert.Equal(600, result); 
        }

        [Fact]
        public void CalculateDeduction_ReturnsZeroForNoDependents()
        {
        
            var calculator = new DependentsBenfitDeductionCalculator();
            var employeeDTO = new EmployeeHoursDTO
            {
                Dependents = new List<DependentDto>()
            };        
            var result = calculator.CalculateDeduction(employeeDTO);           
            Assert.Equal(0, result); 
        }

        [Fact]
        public void CalculateDeduction_ElderlyBenifitCorrectDeduction()
        {
            // Arrange
            var calculator = new ElderlyBenfitDeductionCalculator();
            var today = DateTime.Today;
            var elderlyDependentDOB1 = today.AddYears(-60);
            var elderlyDependentDOB2 = today.AddYears(-55);
            var employeeDTO = new EmployeeHoursDTO
            {
                Dependents = new List<DependentDto>
                {
                    new DependentDto { DateOfBirth = elderlyDependentDOB1 },
                    new DependentDto { DateOfBirth = elderlyDependentDOB2 }
                }
            };

            // Act
            var result = calculator.CalculateDeduction(employeeDTO);
            Assert.Equal(200, result);
        }

        [Fact]
        public void CalculateDeduction_ElderlyBenifitZeroForNoElderlyDependents()
        {
            // Arrange
            var calculator = new ElderlyBenfitDeductionCalculator();
            var today = DateTime.Today;
            var nonElderlyDependentDOB = today.AddYears(-49); 
            var employeeDTO = new EmployeeHoursDTO
            {
                Dependents = new List<DependentDto>
                {
                    new DependentDto { DateOfBirth = nonElderlyDependentDOB }
                }
            };
            var result = calculator.CalculateDeduction(employeeDTO);

            Assert.Equal(0, result); 
        }
    }

    public class MockStandardEarningCalculator : StandardEarningCalculator
    {
        public override decimal CalculateEarnings(EmployeeHoursDTO employeeHours)
        {           
            return base.CalculateEarnings(employeeHours);
        }
    }



  


    
}
