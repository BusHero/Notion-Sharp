using System;

namespace Notion;

public class NotionException : Exception
{
    public int Status { get; set; }
    public string Code { get; set; }

    public NotionException(string message) : base(message) { }
}
