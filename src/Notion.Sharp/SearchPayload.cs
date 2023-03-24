using System;

namespace Notion;

public record Filter(string Value, string Property);
public record Sort(string Direction, string Timestamp);
public record SearchPayload(
    string? Query = default, 
    Sort? Sort = default, 
    Filter? Filter = default,
    Guid? StartCursor = default, 
    int? PageSize = default);
