using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace Shared.Files.Extensions;
public static class ResultExtensions {
    public static void ThrowIfFailure(this Result result) {
        if(result.IsSuccessful is false) {
            var msg = result.Messages.First();
            throw new CustomException(msg.Code , msg.Message);
        }
    }
    public static TModel? ThrowIfFailure<TModel>(this Result<TModel> result) {
        if(result.IsSuccessful is false) {
            var msg = result.Messages.First();
            throw new CustomException(msg.Code , msg.Message);
        }
        return result.Model;
    }

    public static Result<TModel> ToResult<TModel>(this TModel? model , string modelName) {
        if(model is null) {
            return Result<TModel>.Failed($"The {modelName} object is null");
        }
        return Result<TModel>.Success($"The {modelName} object is founded." , model);
    }
}