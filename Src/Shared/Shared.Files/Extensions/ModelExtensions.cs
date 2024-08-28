using Shared.Files.Exceptions;

namespace Shared.Files.Extensions;
public static class ModelExtensions {
    public static TModel ThrowIfNull<TModel>(this TModel? model , string messageFormat , params string[] names) {
        if(model is null) {
            throw new CustomException("Null-Object" , string.Format(messageFormat , names));
        }
        return model;
    }

    public static TModel ThrowIfNull<TModel>(this TModel? model , string propertyName) {
        if(model is null) {
            throw new CustomException("Null-Object" , $"The {propertyName} object is null.");
        }
        return model;
    }

    public static TModel? ThrowIfNotNull<TModel>(this TModel? model , string messageFormat , params string[] names) {
        if(model is not null) {
            throw new CustomException("HasValue" , string.Format(messageFormat , names));
        }
        return model;
    }
}
