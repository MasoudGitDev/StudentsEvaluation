using Apps.School.Domains.Students.Commands;
using Apps.School.Domains.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Extensions;
using Shared.Files.Models;
using Shared.Files.Validators.School;

namespace Server.MainApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpGet("GetAll")]
    public async Task<Result<List<StudentDto>>> GetAllAsync([FromQuery] PaginationDto? model) {
        return await _mediator.Send(GetStudents.New(model.Normalize()));
    }

    [HttpGet("GetAverageScore/{nationalCode}")]
    public async Task<Result<StudentMeanScoreDto>> GetAverageScoreAsync(string nationalCode) {
        return await _mediator.Send(new GetStudentMeanScore(nationalCode));
    }

    [HttpGet("GetStudentsAverageScore")]
    public async Task<Result<List<StudentMeanScoreDto>>> GetStudentsAverageScoreAsync(
        [FromQuery] PaginationDto model , bool isDescending = true) {
        return await _mediator.Send(GetStudentsMeanScore.New(model.Normalize() , isDescending));
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] StudentDto model) {
        return await ValidationResult<StudentDtoValidator , StudentDto , Create>(model);
    }
}
