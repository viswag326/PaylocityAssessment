using Api.Dtos.Employee;
using Api.Models;
using Api.Dtos.Dependent;
using Microsoft.OpenApi.Extensions;

namespace Api.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public void AddEmployee(EmployeeDto employee)
        {
            using (var _context = new PaylocityBenefitsContext())
            {
                List<Dependent> _dependents = new List<Dependent>();
                if (employee.Dependents != null && employee.Dependents.Count > 0)
                {
                    foreach(DependentDto dependent in employee.Dependents)
                    {
                        _dependents.Add(
                             new Dependent()
                             {
                                FirstName =  dependent.FirstName,
                                LastName = dependent.LastName,
                                DateOfBirth = dependent.DateOfBirth,
                                RelationShipId = (int) dependent.Relationship
                             });
                    }
                }

                _context.Employees.Add(new Employee
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateOfBirth = employee.DateOfBirth,
                    SalaryPerHour = employee.SalaryPerHour,
                    Dependents = _dependents

                });
               
                _context.SaveChanges();
            }
        }

        public List<EmployeeDto> GetAllEmployees()
        {
            using (var _context = new PaylocityBenefitsContext())
            {
                return _context.Employees
                     .Select(x => new EmployeeDto
                     {
                         EmployeeID = x.Id,
                         FirstName = x.FirstName,
                         LastName = x.LastName,
                         DateOfBirth = x.DateOfBirth,
                         SalaryPerHour = x.SalaryPerHour,
                         Dependents = x.Dependents.Select(d => new DependentDto
                         {
                             FirstName = d.FirstName,
                             LastName = d.LastName,
                             DateOfBirth = d.DateOfBirth,
                             RelationshipDisplayText = d.RelationShip.Name

                         }).ToList()
                     })
                     .ToList();
            }
        }

        
    }
}
