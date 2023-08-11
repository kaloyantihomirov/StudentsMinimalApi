using StudentsMinimalApi;

using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

ConcurrentDictionary<string, Student> _students = new();

app.MapGet("/student", () =>  _students);



app.Run();