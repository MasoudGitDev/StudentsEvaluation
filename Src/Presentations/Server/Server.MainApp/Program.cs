using Apps.School;
using Infra.SqlServerWithEF.Extensions;
using Shared.Files.Validators.School;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

await builder.Services.AddInfraLayerServices();
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssemblies(
        typeof(AppsSchoolAssembly).Assembly
    );
});

// Fluent Validation
builder.Services.AddTransient(service => new StudentDtoValidator());
builder.Services.AddTransient(service => new CourseDtoValidator());
builder.Services.AddTransient(service => new TeacherDtoValidator());
builder.Services.AddTransient(service => new ExamResultDtoValidator());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
