namespace Domain.Models;

public class Workspace
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string RootPath { get; set; } = "";

    public DateTime CreatedUtc { get; set; }

    public DateTime LastModifiedUtc { get; set; }
}
