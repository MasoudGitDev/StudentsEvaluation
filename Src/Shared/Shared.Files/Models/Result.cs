namespace Shared.Files.Models;

public record class Result(bool IsSuccessful , List<MessageDescription> Messages) {
    public Result() : this(false , [MessageDescription.Warning("Init" , "Parameterless constructor")]) {

    }
    public static Result Success(string message)
        => new(true , [MessageDescription.Success("Success" , message)]);
    public static Result Failed(string message)
       => new(false , [MessageDescription.Error("Failure" , message)]);
    public static Result Failed(List<MessageDescription> errors)
       => new(false , errors);
}

//=========== generic<T>
public record class Result<TModel>(bool IsSuccessful , List<MessageDescription> Messages , TModel? Model) {
    public Result()
        : this(false , [MessageDescription.Warning("Init" , "Parameterless constructor")] , default) { }
    public static Result<TModel> Success(string message , TModel model)
        => new(true , [MessageDescription.Success("Success" , message)] , model);
    public static Result<TModel> Warning(string message , TModel model)
      => new(true , [MessageDescription.Warning("Warning" , message)] , model);
    public static Result<TModel> Failed(string message , TModel? model = default)
         => new(false , [MessageDescription.Error("Failure" , message)] , model);
    public static Result<TModel> Failed(List<MessageDescription> errors , TModel? model = default)
      => new(false , errors , model);
}
