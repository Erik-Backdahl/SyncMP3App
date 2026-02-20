using System;

public class Messages
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string MessageContent { get; set; } = null!;
    public DateTime Received {get; set;} 
}