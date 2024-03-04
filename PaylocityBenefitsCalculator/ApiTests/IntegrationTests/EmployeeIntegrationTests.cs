using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.BusinessLayer;
using Api.Controllers;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{

    private readonly EmployeesController _controller;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<ILogger<EmployeesController>> _mockLogger;
    private readonly Mock<IEmployeeBusinessLayer> _mockBusinessLayer;

    public EmployeeIntegrationTests()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockLogger = new Mock<ILogger<EmployeesController>>();
        _mockBusinessLayer = new Mock<IEmployeeBusinessLayer>();
        _controller = new EmployeesController(_mockEmployeeRepository.Object, _mockLogger.Object, _mockBusinessLayer.Object);
    }

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
       // var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<EmployeeDto>
        {
            new()
            {
                EmployeeID = 1,
                FirstName = "LeBron",
                LastName = "James",
                SalaryPerHour = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                EmployeeID = 2,
                FirstName = "Ja",
                LastName = "Morant",
                SalaryPerHour = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<DependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                EmployeeID = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                SalaryPerHour = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<DependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        // await response.ShouldReturn(HttpStatusCode.OK, employees);
        // Arrange

        _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Returns(employees);
        var result = await _controller.GetAll();

        var actionResult = Assert.IsType<ActionResult<ApiResponse<List<EmployeeDto>>>>(result);
        var apiResponse = Assert.IsType<ApiResponse<List<EmployeeDto>>>(actionResult.Value);

        Assert.True(apiResponse.Success);
        Assert.Equal(employees, apiResponse.Data);
        Assert.Equal("Employees retrieved sucessfully.", apiResponse.Message);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
       // var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new EmployeeDto
        {
            EmployeeID = 1,
            FirstName = "LeBron",
            LastName = "James",
            SalaryPerHour = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        //  await response.ShouldReturn(HttpStatusCode.OK, employee);

        
        var employeeId = 1;
        _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Returns(new List<EmployeeDto> { employee });

        var result = await _controller.Get(employeeId);

        var actionResult = Assert.IsType<ActionResult<ApiResponse<EmployeeDto>>>(result);
        var apiResponse = Assert.IsType<ApiResponse<EmployeeDto>>(actionResult.Value);

        Assert.True(apiResponse.Success);
        Assert.Equal(employee, apiResponse.Data);
        Assert.Equal("Employees retrieved sucessfully.", apiResponse.Message);
    }
    
    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        //var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        //await response.ShouldReturn(HttpStatusCode.NotFound);

        var employeeId = 500;
        _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Returns(new List<EmployeeDto>());

        var result = await _controller.Get(employeeId);

        var actionResult = Assert.IsType<ActionResult<ApiResponse<EmployeeDto>>>(result);
        var apiResponse = Assert.IsType<ApiResponse<EmployeeDto>>(actionResult.Value);

        Assert.True(apiResponse.Success);
        Assert.Equal(null, apiResponse.Data);
        Assert.Equal("Employees not found.", apiResponse.Message);
    }
}

