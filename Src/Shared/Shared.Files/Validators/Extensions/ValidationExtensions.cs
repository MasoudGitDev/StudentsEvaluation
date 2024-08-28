using FluentValidation;

namespace Shared.Files.Validators.Extensions;
public static class ValidationExtensions {

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
