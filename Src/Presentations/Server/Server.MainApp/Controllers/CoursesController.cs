using Apps.School.Domains.Courses.Commands;
using Apps.School.Domains.Courses.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Server.MainApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpGet("All")]
    public async Task<Result<List<CourseDto>>> GetAllAsync([FromQuery] PaginationDto? model) {
        return await _mediator.Send(GetCourses.New(model ?? new()));
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] CourseDto model) {
        return await _mediator.Send(model.Adapt<Create>());
    }
}
