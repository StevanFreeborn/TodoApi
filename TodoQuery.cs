using System.Reflection;

namespace TodoApi;

public class TodoQuery
{
  public TodoStatus Status { get; set; }

  public static ValueTask<TodoQuery?> BindAsync(HttpContext context)
  {
    TodoQuery? result = null;

    if (Enum.TryParse<TodoStatus>(context.Request.Query["status"], ignoreCase: true, out var status))
    {
      result = new TodoQuery { Status = status };
    }

    return ValueTask.FromResult(result);
  }
}

public enum TodoStatus
{
  Uncompleted,
  Completed
}