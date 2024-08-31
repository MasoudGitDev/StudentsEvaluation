using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.MainApp.Attributes;
using Shared.Files.Extensions;
using Shared.Files.Models;
using Shared.Files.Validators.Extensions;

namespace Server.MainApp.Controllers;

[ResultException]
[ApiController]
public class SchoolController(IMediator _mediator , IServiceProvider _serviceProvider) : ControllerBase {

    protected static Result ValidateModel<TValidator, TModel>(TValidator validator , TModel model)
       where TValidator : AbstractValidator<TModel> {
        var validation = validator.Validate(model);
        if(!validation.IsValid) {
            return Result.Failed(validation.Errors.AsMessageDescriptions());
        }
        return Result.Success("The Model is valid");
    }

    protected async Task<Result> ValidationResult<TValidator, TModel, TDestinationModel>(TModel model)
        where TValidator : AbstractValidator<TModel>
        where TDestinationModel : IRequest<Result> {
        var validator = (_serviceProvider.GetRequiredService<TValidator>()).ThrowIfNull(typeof(TValidator).Name);
        ValidateModel(validator , model).ThrowIfFailure();
        return await _mediator.Send(model.Adapt<TDestinationModel>());
    }
}
