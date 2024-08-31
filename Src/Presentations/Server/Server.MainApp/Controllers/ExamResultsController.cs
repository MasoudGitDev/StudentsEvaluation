using Apps.School.Domains.ExamResults.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;
using Shared.Files.Validators.School;

namespace Server.MainApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamResultsController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] ExamResultDto model) {
        return await ValidationResult<ExamResultDtoValidator , ExamResultDto , Create>(model);
    }

}

