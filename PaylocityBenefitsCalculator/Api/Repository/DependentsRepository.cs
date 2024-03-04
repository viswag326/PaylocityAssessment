using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Repository
{
    public class DependentsRepository : IDependentsRepository
    {
        public List<DependentDto> GetAllDependents()
        {
            using (var _context = new PaylocityBenefitsContext())
            {
                var employees = _context.Employees
                     .Select(x => new EmployeeDto
                     {
                         Dependents = x.Dependents.Select(d => new DependentDto
                         {
                             FirstName = d.FirstName,
                             LastName = d.LastName,
                             DateOfBirth = d.DateOfBirth,
                             RelationshipDisplayText = d.RelationShip.Name

                         }).ToList()
                     })
                     .ToList();

                return employees.SelectMany(x => x.Dependents).ToList();
            }
        }
    }
}
