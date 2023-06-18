using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(
  options => options.UseInMemoryDatabase("TodoList")
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapGet(
  "/todos",
  async (TodoQuery? query, TodoDb db) =>
  {
    var todos = db.Todos.AsQueryable();

    if (query is not null)
    {
      if (query.Status is TodoStatus.Completed)
      {
        todos = todos.Where(t => t.IsComplete == true);
      }

      if (query.Status is TodoStatus.Uncompleted)
      {
        todos = todos.Where(t => t.IsComplete == false);
      }
    }

    return await todos.ToListAsync();
  }
);

app.MapGet(
  "/todos/{id}",
  async (TodoDb db, int id) =>
    await db.Todos.FindAsync(id) is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound()
);

app.MapPost("/todos", async (Todo todo, TodoDb db) =>
{
  db.Todos.Add(todo);
  await db.SaveChangesAsync();
  return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
  var todo = await db.Todos.FindAsync(id);

  if (todo is null)
  {
    return Results.NotFound();
  }

  todo.Name = inputTodo.Name;
  todo.IsComplete = inputTodo.IsComplete;

  await db.SaveChangesAsync();

  return Results.Ok(todo);
});

app.MapDelete("/todos/{id}", async (int id, TodoDb db) =>
{
  var todo = await db.Todos.FindAsync(id);

  if (todo is null)
  {
    return Results.NotFound();
  }

  db.Todos.Remove(todo);
  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.Run();
