using FluentValidation;
using Shared.Files.DTOs;
using Shared.Files.Validators.Extensions;

namespace Shared.Files.Validators.School;

/// <summary>
/// قوانین هر آیتم دلخواه بود و صرفا جهت اجرای پروژه به صورت دمو محور است
/// </summary>
public class TeacherDtoValidator : AbstractValidator<TeacherDto> {
    public TeacherDtoValidator() {
        RuleFor(item => item.FirstName).NotEmptyWithRange(3 , 50);
        RuleFor(item => item.LastName).NotEmptyWithRange();
        RuleFor(item => item.LastName).NotEmptyWithRange();
    }
}