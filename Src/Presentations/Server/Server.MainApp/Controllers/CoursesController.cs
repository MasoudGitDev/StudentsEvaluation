using Apps.School.Domains.Courses.Commands;
using Apps.School.Domains.Courses.Queries;
using Domains.School.Course.Aggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Server.MainApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(IMediator _mediator) : SchoolController {

    [HttpGet("All")]
    public async Task<Result<List<Course>>> GetAllAsync() {
        return await _mediator.Send(GetCourses.New());
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] CourseDto model) {
        return await _mediator.Send(model.Adapt<Create>());
    }
}
