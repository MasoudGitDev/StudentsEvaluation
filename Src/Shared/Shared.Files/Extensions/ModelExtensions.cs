using Shared.Files.Constants;
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

    public static float MustScoreBeInRange(this float number) {
        if(number >= 0f && number <= 20f) {
            return number;
        }
        throw new CustomException("ScoreRangeError" , String.Format(MessageResults.InvalidScore , number));
    }

    public static DateTime MustDateTimeLessThanNow(this DateTime dateTime) {
        if(dateTime >= DateTime.UtcNow) {
            throw new CustomException("EqualOrBiggerDateTime" , MessageResults.DateTimeError);            
        }
        return dateTime;
    }

    public static string ToFormat(this string text , params string[] args) { 
        return String.Format(text , args);
    }
}
