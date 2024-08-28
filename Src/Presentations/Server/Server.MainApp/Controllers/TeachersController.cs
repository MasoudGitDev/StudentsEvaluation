using Apps.School.Domains.Teachers.Commands;
using Apps.School.Domains.Teachers.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Server.MainApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeachersController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpGet("All")]
    public async Task<Result<List<TeacherDto>>> GetAllAsync(PaginationDto model) {
        return await _mediator.Send(GetTeachers.New(model ?? new()));
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] TeacherDto model) {
        return await _mediator.Send(model.Adapt<Create>());
    }
}
