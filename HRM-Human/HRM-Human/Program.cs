using Microsoft.EntityFrameworkCore;
using HRM_Human;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/todoitems", async (TodoDb db) =>
     await db.Todos.ToListAsync());


app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
     await db.Todos.FindAsync(id)
     is Todo todo
     ? Results.Ok(todo)
     : Results.NotFound());
app.MapGet("/todoitems/search/{Name}", async (string name,TodoDb db) =>
{
    var employees = await db.Todos
        .Where(e => e.Name!.Contains(name))
        .ToListAsync();

    if (employees is null || employees.Count == 0) return Results.NotFound();

    return Results.Ok(employees);
});
app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.EmployeeID = inputTodo.EmployeeID;
    todo.Name = inputTodo.Name;
    todo.PhoneNumber = inputTodo.PhoneNumber;
    todo.Address = inputTodo.Address;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});
app.Run();