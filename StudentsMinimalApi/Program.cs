using System.Collections.Concurrent;

using StudentsMinimalApi;
using StudentsMinimalApi.Validation;

///<remarks>
///Here we register the IProblemDetails service in the dependency injection container.
///</remarks>
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddSingleton<StudentService>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

if (app.Environment.IsDevelopment())
{
    ///<remarks>
    ///This is a way to obtain the collection of endpoints describing our application.
    ///</remarks>
    app.MapGet("/routes", (EndpointDataSource endpointSource) =>
        string.Join(Environment.NewLine, endpointSource.Endpoints));
}

///<remarks>
///Any response that reaches the Status code pages middleware with an error status code and
///doesn't already have a body has a Problem Details body added by the
///middleware. The middleware converts all error responses automatically,
///regardless of whether they were generated by an endpoint or from other
///middleware.
///</remarks>
app.UseStatusCodePages();

RouteGroupBuilder studentsApi = app.MapGroup("/student");

studentsApi.MapGet("/", () => StudentService.AllStudents).WithName("allStudents");

RouteGroupBuilder studentsApiWithValidation = studentsApi
    .MapGroup("/")
    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

studentsApiWithValidation.MapGet("/{id}", (StudentService studentService, string id) =>
{
    Student? student = studentService.GetStudent(id);

    return student is null
        ? Results.Problem(statusCode: 404)
        : TypedResults.Ok(student);
}).WithName("getStudentById");


studentsApiWithValidation.MapPost("/{id}",
    (StudentService studentService,
    LinkGenerator links,
    Student student,
    string id) =>
        studentService.AddStudent(id, student)
               ? TypedResults.Created(links.GetPathByName("getStudentById", new { id }) ?? "Something went wrong when trying to generate the location path.", student)
               : Results.ValidationProblem(new Dictionary<string, string[]>
                {
                 { "id", new[] { "A student with the given id already exists." } }
                })).WithName("addStudent");

/// <remarks>
/// Note that the put method call will either update an existing student 
/// or create a new one.
/// </remarks>
studentsApiWithValidation.MapPut("/{id}", 
    (StudentService studentService,
     string id,
     Student student) =>
{
    studentService.UpdateStudent(id, student);

    return TypedResults.NoContent();
}).WithName("updateStudent");

studentsApiWithValidation.MapDelete("/{id}", (StudentService studentService, string id) =>
{
    studentService.DeleteStudent(id);

    return TypedResults.NoContent();
}).WithName("deleteStudent");

app.Run();


/// <summary>
/// I added this class only for testing purposes! (for the WebApplicationFactory)
/// </summary>
public partial class Program { }