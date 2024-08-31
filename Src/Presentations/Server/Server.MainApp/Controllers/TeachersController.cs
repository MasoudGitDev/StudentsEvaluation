using Apps.School.Domains.Teachers.Commands;
using Apps.School.Domains.Teachers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Extensions;
using Shared.Files.Models;
using Shared.Files.Validators.School;

namespace Server.MainApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeachersController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpGet("GetAll")]
    public async Task<Result<List<TeacherDto>>> GetAllAsync([FromQuery] PaginationDto model) {
        return await _mediator.Send(GetTeachers.New(model.Normalize()));
    }

    [HttpGet("GetTeacherPerformance/{personnelCode}")]
    public async Task<Result<List<TeacherPerformanceDto>>> GetTeacherPerformanceAsync([FromRoute] string personnelCode) {
        return await _mediator.Send(GetTeacherPerformance.New(personnelCode));
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] TeacherDto model) {
        return await ValidationResult<TeacherDtoValidator , TeacherDto , Create>(model);
    }
}
