using FluentValidation;
using Shared.Files.DTOs;
using Shared.Files.Validators.Extensions;

namespace Shared.Files.Validators.School;

/// <summary>
/// قوانین هر آیتم دلخواه بود و صرفا جهت اجرای پروژه به صورت دمو محور است
/// </summary>
public class CourseDtoValidator : AbstractValidator<CourseDto> {
    public CourseDtoValidator() {
        RuleFor(item => item.Code).NotEmptyWithRange(3 , 50);
        RuleFor(item => item.Name).NotEmptyWithRange();
        RuleFor(item => item.TeacherPersonnelCode).NotEmptyWithRange();
    }
}