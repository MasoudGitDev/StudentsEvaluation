using Apps.School.Domains.Students.Commands;
using Apps.School.Domains.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;
using Shared.Files.Validators.School;

namespace Server.MainApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController(IMediator _mediator , IServiceProvider _serviceProvider)
    : SchoolController(_mediator , _serviceProvider) {

    [HttpGet("All")]
    public async Task<Result<List<StudentDto>>> GetAllAsync([FromQuery] PaginationDto? model) {
        var (usePagination , pageNumber , pageSize) = model ?? new PaginationDto(true , 1 , 50);
        return await _mediator.Send(GetStudents.New(usePagination,pageNumber,pageSize));
    }

    [HttpGet("CalculateAverageScore/{nationalCode}")]
    public async Task<Result<StudentMeanScoreDto>> CalculateAverageScoreAsync(string nationalCode) {
        return await _mediator.Send(new GetStudentMeanScore(nationalCode));
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] StudentDto model) {
        return await ValidationResult<StudentDtoValidator , StudentDto , Create>(model);
    }



}
