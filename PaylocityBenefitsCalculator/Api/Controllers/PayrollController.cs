using Api.BusinessLayer;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {

        private IPayrollRepository _payrollRepository;

        private IPayrollBusinessLayer _payrollBusinessLayer;

        private readonly ILogger<PayrollController> _logger;

        public PayrollController(IPayrollRepository payrollRepository, IPayrollBusinessLayer payrollBusinessLayer, ILogger<PayrollController> logger)
        {
            _payrollRepository = payrollRepository;
            _payrollBusinessLayer = payrollBusinessLayer;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Add Employee")]
        [HttpPost("createpayperiodschedule")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> CreatePayPeriodSchedule(int year)
        {
            try
            {
                _payrollRepository.CreatePayPeriodSchedule(year);
                return Ok(new ApiResponse<EmployeeDto>
                {
                    Message = "Pay period schedule created successfully.",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Payroll controller CreatePayPeriodSchedule method ", ex);
                return Ok(new ApiResponse<EmployeeDto>
                {
                    Error = "An error occured. If the problem persists, please contact admin."
                });
            }

        }


        [SwaggerOperation(Summary = "Add Employee Working Hours")]
        [HttpPost("addemployeeworkinghours")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDto>>>> AddEmployeeWorkingHours([FromBody] List<EmployeeHoursDTO> employeeHours)
        {
            try
            {
                string message =  _payrollRepository.AddEmployeeWorkingHours(employeeHours);
                return Ok(new ApiResponse<EmployeeDto>
                {
                    Message = message,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Customer controller AddEmployeeWorkingHours method ", ex);
                return Ok(new ApiResponse<EmployeeDto>
                {
                    Error = "An error occured. If the problem persists, please contact admin."

                });
            }

        }

        [SwaggerOperation(Summary = "Add Employee Working Hours")]
        [HttpPost("processpayroll")]
        public async Task<ActionResult<ApiResponse<List<EmployeePaymentDTO>>>> ProcessPayroll(DateTime payPeriodStartDate, DateTime payPeriodEndDate)
        {
            try
            {
                List<EmployeeHoursDTO> employeeHoursDTO = _payrollRepository.GetEmployeeDetailsForProcessingPayroll(payPeriodStartDate, payPeriodEndDate);
                List<EmployeePaymentDTO> employeePaymentsDTO = _payrollBusinessLayer.ProcessPayroll(employeeHoursDTO);
                _payrollRepository.UpdatePayrollDataInDB(employeePaymentsDTO, payPeriodStartDate, payPeriodEndDate);
                return Ok(new ApiResponse<List<EmployeePaymentDTO>>
                {
                    Message = "Payroll processed successfully.",
                    Success = true,
                    Data = employeePaymentsDTO
                });
            }
            catch (Exception ex)
            {
                //  _logger.LogError("Error occured in Customer controller GetProductsByCategory method ", ex);
                return Ok(new ApiResponse<List<EmployeePaymentDTO>>
                {
                    Error = "An error occured. If the problem persists, please contact admin."

                });
            }

        }
    }
}
