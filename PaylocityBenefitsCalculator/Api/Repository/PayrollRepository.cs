using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Api.Repository
{
    public class PayrollRepository : IPayrollRepository
    {
        public string AddEmployeeWorkingHours(List<EmployeeHoursDTO> employeeHours)
        {
            string addedRecords = "Hours logged successfully for employees: ";
            string failedRecords = "Hours failed to log for employees: ";
            using (var _context = new PaylocityBenefitsContext())
            {
                foreach (var employeeHoursDTO in employeeHours)
                {
                    var payPeriodSchedule = _context.PayPeriodSchedules.Where(x => x.StartDate == employeeHoursDTO.PayPeriodStartDate
                       && x.EndDate == employeeHoursDTO.PayPeriodEndDate).FirstOrDefault();
                    if (payPeriodSchedule != null && payPeriodSchedule.Id > 0)
                    {
                        _context.EmployeePayments.Add(
                             new EmployeePayment()
                             {
                                 EmployeeId = employeeHoursDTO.EmployeeID,
                                 RegularHours = employeeHoursDTO.RegularHours,
                                 OverTimeHours = employeeHoursDTO.OverTimeHours,
                                 PayPeriodScheduleId = payPeriodSchedule.Id
                             }
                            );
                        _context.SaveChanges();
                        addedRecords = addedRecords + " Employee ID: " +
                            employeeHoursDTO.EmployeeID + " Period Start " + employeeHoursDTO.PayPeriodStartDate +
                            " Period End Date: " + employeeHoursDTO.PayPeriodEndDate + ",";
                    }
                    else
                    {
                        failedRecords = failedRecords + " Employee ID: " +
                            employeeHoursDTO.EmployeeID + " Period Start " + employeeHoursDTO.PayPeriodStartDate +
                            " Period End Date: " + employeeHoursDTO.PayPeriodEndDate + ",";
                    }
                }
            }

            return addedRecords + failedRecords;
        }

        public void CreatePayPeriodSchedule(int year)
        {

            DateTime startDate = new DateTime(year, 1, 1);
            while (startDate.DayOfWeek != DayOfWeek.Friday)
            {
                startDate = startDate.AddDays(1);
            }

            while (startDate.Year == year)
            {
                DateTime payStartDate = startDate;
                DateTime payEndDate = startDate.AddDays(13);

                Console.WriteLine("Pay Period: {0} - {1}", payStartDate.ToString("MM/dd/yyyy"), payEndDate.ToString("MM/dd/yyyy"));

                using (var _context = new PaylocityBenefitsContext())
                {
                    if (!_context.PayPeriodSchedules.Any(x => x.StartDate == payStartDate && x.EndDate == payEndDate))
                    {
                        _context.PayPeriodSchedules.Add(
                            new PayPeriodSchedule()
                            {
                                StartDate = payStartDate,
                                EndDate = payEndDate
                            });
                        _context.SaveChanges();
                    }
                }

                startDate = startDate.AddDays(14);
            }
        }

        public List<EmployeeHoursDTO> GetEmployeeDetailsForProcessingPayroll(DateTime payPeriodStartDate, DateTime payPeriodEndDate)
        {
            List<EmployeeHoursDTO> result = new List<EmployeeHoursDTO>();
            using (var _context = new PaylocityBenefitsContext())
            {
                int payPeriodScheduleId = _context.PayPeriodSchedules
                                         .Where(x => x.StartDate == payPeriodStartDate && x.EndDate == payPeriodEndDate).Select(x => x.Id).FirstOrDefault();
                List<int> employeeIDs = _context.PayPeriodSchedules
                                         .Where(x => x.Id == payPeriodScheduleId)
                                         .SelectMany(x => x.EmployeePayments.Select(ep => ep.EmployeeId))
                                         .ToList();

                foreach (int employeeId in employeeIDs)
                {
                    var employee = _context.Employees.Where(x => x.Id == employeeId).FirstOrDefault();
                    var employeeHours = _context.EmployeePayments.Where(x => x.EmployeeId == employeeId && x.PayPeriodScheduleId == payPeriodScheduleId).FirstOrDefault();
                    List<Dependent> employeeDependents = _context.Employees.Where(x => x.Id == employeeId).SelectMany(x => x.Dependents).ToList();
                    List<DependentDto> dependents = new List<DependentDto>();
                    if (employeeDependents != null && employeeDependents.Count > 0)
                    {
                        foreach (var dependent in employeeDependents)
                        {
                            dependents.Add(new DependentDto() { DateOfBirth = dependent.DateOfBirth, Relationship = (Relationship)dependent.RelationShipId });
                        }

                    }
                    result.Add(new EmployeeHoursDTO()
                    {
                        EmployeeID = employeeId,
                        RegularHours = employeeHours.RegularHours,
                        OverTimeHours = employeeHours.OverTimeHours,
                        SalaryPerHour = employee.SalaryPerHour,
                        Dependents = dependents,
                        PayPeriodEndDate = payPeriodEndDate,
                        PayPeriodStartDate = payPeriodStartDate
                    });
                }

            }
            return result;
        }

        public void UpdatePayrollDataInDB(List<EmployeePaymentDTO> employeePaymentsDTO, DateTime payPeriodStartDate, DateTime payPeriodEndDate)
        {
            using (var _context = new PaylocityBenefitsContext())
            {
                int payPeriodScheduleId = _context.PayPeriodSchedules
                                         .Where(x => x.StartDate == payPeriodStartDate && x.EndDate == payPeriodEndDate)
                                         .Select(x => x.Id).FirstOrDefault();

                foreach(EmployeePaymentDTO employeePayment in employeePaymentsDTO)
                {
                    int employeePaymentId = _context.EmployeePayments
                                                .Where(x => x.PayPeriodScheduleId == payPeriodScheduleId && x.EmployeeId == employeePayment.EmployeeID)
                                                .Select(x => x.Id).FirstOrDefault();
                    var _employeePayment = new EmployeePayment()
                    {
                        Id = employeePaymentId,
                        BaseBenefits = employeePayment.BaseBenifits,
                        DependentBenefits = employeePayment.DependentBenefits,
                        AddOnBenefits = employeePayment.AddOnBenefits,
                        ElderlyBenefits = employeePayment.ElederyBenefits,
                        FederalTax = employeePayment.FederalTax,
                        StateTax = employeePayment.StateTax,
                        TotalEarnings = employeePayment.TotalEarnings,
                        TotalDeductions = employeePayment.TotalDeductions,
                        NetPay = employeePayment.NetPay 

                    };
                    _context.EmployeePayments.Attach(_employeePayment);
                    _context.Entry(_employeePayment).Property(x => x.BaseBenefits).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.DependentBenefits).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.AddOnBenefits).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.ElderlyBenefits).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.FederalTax).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.StateTax).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.TotalEarnings).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.TotalDeductions).IsModified = true;
                    _context.Entry(_employeePayment).Property(x => x.NetPay).IsModified = true;

                    _context.SaveChanges();
                }

                var _payPeriodSchedule = new PayPeriodSchedule()
                {
                    Id = payPeriodScheduleId,
                    IsExecuted = true

                };
                _context.PayPeriodSchedules.Attach(_payPeriodSchedule);
                _context.Entry(_payPeriodSchedule).Property(x => x.IsExecuted).IsModified = true;
                _context.SaveChanges();
            }
          }
    }
}
