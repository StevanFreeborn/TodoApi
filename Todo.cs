using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApi;

public class Todo
{
  public int Id { get; set; }

  [Required]
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  [JsonPropertyName("isComplete")]
  public bool IsComplete { get; set; }
}