using Shared.Files.Exceptions;
using Shared.Files.ValueObjects;

namespace Shared.Files.Extensions {
    public static class ModelExtensions {
        public static TModel ThrowIfNull<TModel>(TModel? model , string propertyName) {
            if(model is null) {
                throw new CustomException("Null-Object" , $"The {propertyName} object is null.");
            }
            return model;
        }
        public static TModel? ThrowIfNotNull<TModel>(TModel? model , string propertyName) {
            if(model is not null) {
                throw new CustomException("HasValue" , $"The {propertyName} object must be null.");
            }
            return model;
        }

        public static TModel ThrowIfNull<TModel>(TModel? model , Description description) {
            if(model is null) {
                throw new CustomException("Null-Object" , description);
            }
            return model;
        }
        public static TModel? ThrowIfNotNull<TModel>(TModel? model , Description description) {
            if(model is not null) {
                throw new CustomException("HasValue" , description);
            }
            return model;
        }
    }
}
