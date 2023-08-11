using Microsoft.AspNetCore.Builder;
using StudentsMinimalApi;

using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

ConcurrentDictionary<string, Student> _students = new();

app.MapGet("/student", () =>  _students);

app.MapGet("student/{id}", (string id) => _students[id]);

app.MapPost("/student/{id}", (string id, Student student) =>
{
    _students[id] = student;
});

app.MapPut("/student/{id}", (string id, Student student) =>
{
    _students[id] = student;
});

app.MapDelete("/student/{id}", (string id) =>
{
    _students.TryRemove(id, out _);
});

app.Run();