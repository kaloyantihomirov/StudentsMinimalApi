using StudentsMinimalApi;

using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

///<remarks>
///This middleware combined with the service we registered will allow us 
///to standardize our error handling and have better communication 
///with API clients.
///</remarks>
app.UseStatusCodePages();

ConcurrentDictionary<string, Student> _students = new();

app.MapGet("/student", () => _students);

app.MapGet("student/{id}", (string id) =>
     _students.TryGetValue(id, out var student)
        ? TypedResults.Ok(student)
        : Results.Problem(statusCode: 404)
);

app.MapPost("/student/{id}", (string id, Student student) =>
  _students.TryAdd(id, student) ?
  TypedResults.Created($"/student/{id}", student) :
  Results.ValidationProblem(new Dictionary<string, string[]> 
  {
    {
      "id",
      new [] { "A student with the given id already exists." }
    }
  }));

/// <remarks>
/// Note that the put method call will either update an existing student 
/// or create a new one.
/// </remarks>
app.MapPut("/student/{id}", (string id, Student student) =>
{
    _students[id] = student;

    return TypedResults.NoContent();
});

app.MapDelete("/student/{id}", (string id) =>
{
    _students.TryRemove(id, out _);

    return TypedResults.NoContent();
});

app.Run();