using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

//Register DI - AddService
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

//Configure pipeline - Use methods.

//Get todos
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

//Get todo by id
app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id));

//Add a todo
app.MapPost("/todoitems", async (TodoItem todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

//Edit a todo
app.MapPut("/todoitems/{id}", async (int id, TodoItem item, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo == null) return Results.NotFound();
    todo.Name = item.Name;
    todo.IsCompleted = item.IsCompleted;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//Edit a todo
app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if(await db.Todos.FindAsync(id) is TodoItem todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();
