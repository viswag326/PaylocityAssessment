using Api.BusinessLayer;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeesController> _logger;
    private IEmployeeBusinessLayer _employeeBusinessLayer;

    public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger, IEmployeeBusinessLayer employeeBusinessLayer)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _employeeBusinessLayer = employeeBusinessLayer;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> Get(int id)
    {
        var result = new ApiResponse<EmployeeDto>();
        try
        {
            EmployeeDto employee = _employeeRepository.GetAllEmployees().Where(x => x.EmployeeID == id).FirstOrDefault();

            if (employee == null)
            {
                var resultnotFound = new ApiResponse<EmployeeDto>
                {
                    Message = "Employees not found."
                };
                return resultnotFound;
            }

            result = new ApiResponse<EmployeeDto>
            {
                Data = employee,
                Success = true,
                Message = "Employees retrieved sucessfully."
            };

            
        }
        catch(Exception ex)
        {
            _logger.LogError("Error occured in EmployeesController controller Get employee by id ", ex);
            result = new ApiResponse<EmployeeDto>
            {
                Error = "An error occured. If the problem persists, please contact admin.",
            };

            
        }
        return result;

    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("getallemployees")]
    public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach

        var result = new ApiResponse<List<EmployeeDto>>();
        try
        {
            List<EmployeeDto> employess = _employeeRepository.GetAllEmployees();
            result = new ApiResponse<List<EmployeeDto>>
            {
                Data = employess,
                Success = true,
                Message = "Employees retrieved sucessfully."
            };
        }
        catch(Exception ex)
        {
            _logger.LogError("Error occured in EmployeesController controller Get All Employees ", ex);
            result = new ApiResponse<List<EmployeeDto>>
            {
                Error = "An error occured. If the problem persists, please contact admin.",
            };
        }

        return result;
    }

    [SwaggerOperation(Summary = "Add Employee")]
    [HttpPost("addemployee")]
    public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> AddEmployee(EmployeeDto employee)
    {
        var result = new ApiResponse<List<EmployeeDto>>();
        try
        {
            var employeeValidationResult = _employeeBusinessLayer.ValidateEmployee(employee);
            if(!employeeValidationResult.isValidationSuccessfull)
            {
                result = new ApiResponse<List<EmployeeDto>>
                {
                    Message = "Unable to add employee.",
                    Success = false,
                    Error = employeeValidationResult.ErrorMessage
                };
                return result;
            }

            _employeeRepository.AddEmployee(employee);
            result = new ApiResponse<List<EmployeeDto>>
            {
                 Message = "Employee Added Successfully.",
                 Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured in EmployeesController controller AddEmployee ", ex);
            result = new ApiResponse<List<EmployeeDto>>
            {
                Error = "An error occured. If the problem persists, please contact admin.",
            };
        }
        return result;


    }
}
