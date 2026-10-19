using System.ComponentModel.DataAnnotations;
using SimpleApiWithCopilot.Middleware;
using SimpleApiWithCopilot.Models;

var builder = WebApplication.CreateBuilder(args);

// Built-in console logging is enabled by default in ASP.NET Core.
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Custom middleware logs each request and its response status/duration.
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/", () => Results.Ok(new
{
    name = "Simple API with Copilot",
    message = "API is running. See /api/tasks for the CRUD endpoints.",
    endpoints = new[]
    {
        "GET /api/tasks",
        "GET /api/tasks/{id}",
        "POST /api/tasks",
        "PUT /api/tasks/{id}",
        "DELETE /api/tasks/{id}"
    }
}));

// In-memory storage keeps this starter project simple.
// Data resets whenever the application restarts.
var tasks = new List<TaskItem>
{
    new() { Id = 1, Title = "Explore the API", Description = "Send a GET request to /api/tasks.", IsCompleted = false },
    new() { Id = 2, Title = "Test validation", Description = "Try creating a task with an empty title.", IsCompleted = false }
};
var nextId = 3;

// READ: return all tasks.
app.MapGet("/api/tasks", () => Results.Ok(tasks))
   .WithName("GetTasks");

// READ: return one task or 404.
app.MapGet("/api/tasks/{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is null ? Results.NotFound(new { message = $"Task with id {id} was not found." }) : Results.Ok(task);
})
.WithName("GetTaskById");

// CREATE: validate the incoming task before saving it.
app.MapPost("/api/tasks", (TaskInput input) =>
{
    var errors = ValidateInput(input);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    var task = new TaskItem
    {
        Id = nextId++,
        Title = input.Title.Trim(),
        Description = input.Description?.Trim() ?? string.Empty,
        IsCompleted = input.IsCompleted
    };

    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
})
.WithName("CreateTask");

// UPDATE: replace editable fields on an existing task.
app.MapPut("/api/tasks/{id:int}", (int id, TaskInput input) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
        return Results.NotFound(new { message = $"Task with id {id} was not found." });

    var errors = ValidateInput(input);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    task.Title = input.Title.Trim();
    task.Description = input.Description?.Trim() ?? string.Empty;
    task.IsCompleted = input.IsCompleted;

    return Results.Ok(task);
})
.WithName("UpdateTask");

// DELETE: remove an existing task.
app.MapDelete("/api/tasks/{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
        return Results.NotFound(new { message = $"Task with id {id} was not found." });

    tasks.Remove(task);
    return Results.NoContent();
})
.WithName("DeleteTask");

app.Run();

static Dictionary<string, string[]> ValidateInput(TaskInput input)
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(input);
    var isValid = Validator.TryValidateObject(input, context, validationResults, validateAllProperties: true);

    if (isValid)
        return new Dictionary<string, string[]>();

    return validationResults
        .SelectMany(result =>
            (result.MemberNames.Any() ? result.MemberNames : new[] { "request" })
            .Select(member => new { Member = member, Error = result.ErrorMessage ?? "Invalid value." }))
        .GroupBy(item => item.Member)
        .ToDictionary(group => group.Key, group => group.Select(item => item.Error).ToArray());
}
