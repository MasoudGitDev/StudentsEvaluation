using Apps.School.Domains.ExamResults.Commands;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Server.MainApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExamResultsController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] ExamResultDto model) {
        return await _mediator.Send(model.Adapt<Create>());
    }

}

