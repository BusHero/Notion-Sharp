using System;

namespace Notion;

public class NotionException : Exception
{
    public required int Status { get; init; }
    public required string Code { get; init; }

    public NotionException(string message) : base(message) { }
}
