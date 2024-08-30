using FluentValidation;
using FluentValidation.Results;
using Shared.Files.Models;

namespace Shared.Files.Validators.Extensions;
public static class ValidationExtensions {

    public static List<MessageDescription> AsMessageDescriptions(this List<ValidationFailure> validationFailures) {
        List<MessageDescription> errors = [];
        foreach(ValidationFailure failure in validationFailures) {
            errors.Add(MessageDescription.Error(failure.ErrorCode,failure.ErrorMessage));
        }
        return errors;
    }

    public static IRuleBuilderOptions<T , string> NotEmptyWithRange<T>(
        this IRuleBuilder<T , string> ruleBuilder ,
        int min = 3 ,
        int max = 50) {
        return ruleBuilder
            .NotEmpty().WithMessage("Please enter a valid {PropertyName}.")
            .MinimumLength(min).WithMessage("Please enter at least " + min + " valid characters for {PropertyName}")
            .MaximumLength(max).WithMessage("The maximum length of {PropertyName} is" + max + ".");
    } 
}
