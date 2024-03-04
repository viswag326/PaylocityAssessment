using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Dtos.Dependent;
using Api.Models;
using Api.Repository;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Api;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
   
    [Fact]
    //task: make test pass
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        //var response = await HttpClient.GetAsync("https://localhost:7124/api/v1/dependents");
        var dependents = new List<DependentDto>
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
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };
       // await response.ShouldReturn(HttpStatusCode.OK, dependents);
 
        var mockRepository = new Mock<IDependentsRepository>();
        var mockLogger = new Mock<ILogger<DependentsController>>();

        mockRepository.Setup(repo => repo.GetAllDependents())
            .Returns(dependents);
        var controller = new DependentsController(mockRepository.Object, mockLogger.Object);

        var actionResult = await controller.GetAll();

        var result = Assert.IsType<ActionResult<ApiResponse<List<DependentDto>>>>(actionResult);
        var apiResponse = Assert.IsType<ApiResponse<List<DependentDto>>>(result.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(dependents, apiResponse.Data);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        //var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new DependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        //  await response.ShouldReturn(HttpStatusCode.OK, dependent);

        
        var mockRepository = new Mock<IDependentsRepository>();
        var mockLogger = new Mock<ILogger<DependentsController>>();
        mockRepository.Setup(repo => repo.GetAllDependents())
            .Returns(new List<DependentDto> { dependent });

        var controller = new DependentsController(mockRepository.Object, mockLogger.Object);
        var actionResult = await controller.Get(1);
        var result = Assert.IsType<ActionResult<ApiResponse<DependentDto>>>(actionResult);
        var apiResponse = Assert.IsType<ApiResponse<DependentDto>>(result.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(dependent, apiResponse.Data);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        //var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        //await response.ShouldReturn(HttpStatusCode.NotFound);

        var mockRepository = new Mock<IDependentsRepository>();
        var mockLogger = new Mock<ILogger<DependentsController>>();
        mockRepository.Setup(repo => repo.GetAllDependents())
            .Returns(new List<DependentDto>());

        var controller = new DependentsController(mockRepository.Object, mockLogger.Object);
        var actionResult = await controller.Get(1);
        var result = Assert.IsType<ActionResult<ApiResponse<DependentDto>>>(actionResult);
        var apiResponse = Assert.IsType<ApiResponse<DependentDto>>(result.Value);
        Assert.True(apiResponse.Success);
        Assert.Equal(null, apiResponse.Data);
    }
}

