using System.Collections.Generic;

class GenericResponse
{
    public Dictionary<string, string?> Headers { get; set; } = new();
    public string Message { get; set; } = null!;
}