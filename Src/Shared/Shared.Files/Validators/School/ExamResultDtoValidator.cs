using FluentValidation;
using Shared.Files.DTOs;
using Shared.Files.Validators.Extensions;

namespace Shared.Files.Validators.School;
/// <summary>
/// قوانین هر آیتم دلخواه بود و صرفا جهت اجرای پروژه به صورت دمو محور است
/// </summary>
public class ExamResultDtoValidator :AbstractValidator<ExamResultDto> {
    public ExamResultDtoValidator() {
        RuleFor(item => item.Score).WithClosedRange(0 , 20);
        RuleFor(item => item.ExamDateTime)
            .LessThan(DateTime.UtcNow)
            .WithMessage("The datetime must be less than DateTime.UtcNow !");
        RuleFor(item => item.TeacherPersonnelCode).NotEmptyWithRange(3 , 50);
        RuleFor(item => item.CourseCode).NotEmptyWithRange();
        RuleFor(item => item.StudentNationalCode).NotEmptyWithRange();
    
    }
}