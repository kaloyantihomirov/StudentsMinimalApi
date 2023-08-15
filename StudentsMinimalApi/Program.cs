using StudentsMinimalApi;
using StudentsMinimalApi.Validation;
using System.Collections.Concurrent;

///<remarks>
///Here we register the IProblemDetails service in the dependency injection container.
///</remarks>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

if (app.Environment.IsDevelopment())
{
    //app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
    //    string.Join(
    //        Environment.NewLine, endpointSources.SelectMany(source => source.Endpoints)));

    ///<remarks>
    ///This is a way to obtain the collection of endpoints describing our application.
    ///</remarks>
    app.MapGet("/routes", (EndpointDataSource endpointSource) =>
        string.Join(Environment.NewLine, endpointSource.Endpoints));
}

///<remarks>
///This middleware combined with the service we registered will allow us 
///to standardize our error handling and have better communication 
///with API clients. It uses the IProblemDetailsService to generate a Problem Details
///JSON response.
///[QUOTE FROM `ASP.NET Core IN ACTION`, by A. L.] 
///Any response that reaches the middleware with an error status code and
///doesn't already have a body has a Problem Details body added by the
///middleware. The middleware converts all error responses automatically,
///regardless of whether they were generated by an endpoint or from other
///middleware.
///</remarks>
app.UseStatusCodePages();

ConcurrentDictionary<string, Student> _students = new();

RouteGroupBuilder studentsApi = app.MapGroup("/student");

studentsApi.MapGet("/", () => _students);

RouteGroupBuilder studentsApiWithValidation = studentsApi
    .MapGroup("/")
    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

studentsApiWithValidation.MapGet("/{id}", (string id) =>
    _students.TryGetValue(id, out var student)
       ? TypedResults.Ok(student)
       : Results.Problem(statusCode: 404));

studentsApiWithValidation.MapPost("/{id}", (Student student, string id) =>
  _students.TryAdd(id, student)
     ? TypedResults.Created($"/student/{id}", student)
     : Results.ValidationProblem(new Dictionary<string, string[]>
  {
    { "id", new[] { "A student with the given id already exists." } }
  }));

/// <remarks>
/// Note that the put method call will either update an existing student 
/// or create a new one.
/// </remarks>
studentsApiWithValidation.MapPut("/{id}", (string id, Student student) =>
{
    _students[id] = student;

    return TypedResults.NoContent();
});

studentsApiWithValidation.MapDelete("/{id}", (string id) =>
{
    _students.TryRemove(id, out _);

    return TypedResults.NoContent();
});

app.Run();

public partial class Program { }