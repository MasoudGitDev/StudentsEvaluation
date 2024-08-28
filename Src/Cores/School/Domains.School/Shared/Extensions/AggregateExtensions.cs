using Domains.School.Abstractions;
using Shared.Files.Exceptions;

namespace Domains.School.Shared.Extensions;
public static class AggregateExtensions {
    public static ISchoolUOW MustHasValue(this ISchoolUOW? unitOfWork) {
        if(unitOfWork is null){
            throw new CustomException("Null-Object" , "The School unit of work must has value.");
        }
        return unitOfWork;
    }
}
