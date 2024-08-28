using FluentValidation;
using Shared.Files.DTOs;
using Shared.Files.Validators.Extensions;

namespace Shared.Files.Validators.School;

/// <summary>
/// قوانین هر آیتم دلخواه بود و صرفا جهت اجرای پروژه به صورت دمو محور است
/// </summary>
public class StudentDtoValidator : AbstractValidator<StudentDto> {
    public StudentDtoValidator() {
        RuleFor(item => item.FirstName).NotEmptyWithRange(3 , 50);
        RuleFor(item => item.LastName).NotEmptyWithRange(3 , 50);
        RuleFor(item => item.NationalCode).NotEmptyWithRange(3 , 50);
    }
}