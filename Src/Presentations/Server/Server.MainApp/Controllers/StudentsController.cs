using Apps.School.Domains.Students.Commands;
using Apps.School.Domains.Students.Queries;
using Domains.School.Student.Aggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Server.MainApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController(IMediator _mediator) : SchoolController {


    [HttpGet("All")]
    public async Task<Result<List<Student>>> GetAllAsync() {
        return await _mediator.Send(GetStudents.New());
    }

    [HttpPost("Create")]
    public async Task<Result> CreateAsync([FromBody] StudentDto model) {
        return await _mediator.Send(model.Adapt<Create>());
    }

    [HttpPost("CalculateAverageScore/{nationalCode}")]
    public async Task<Result<StudentMeanScoreDto>> CalculateAverageScoreAsync(string nationalCode) {
        return await _mediator.Send(new GetStudentMeanScore(nationalCode));
    }



}
