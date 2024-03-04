using Api.Dtos.Employee;
namespace Api.Repository
{
    public interface IEmployeeRepository
    {
        void AddEmployee(EmployeeDto employee);

        List<EmployeeDto> GetAllEmployees();
    }
}
