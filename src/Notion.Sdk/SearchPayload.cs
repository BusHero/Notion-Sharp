
using System;

namespace Notion
{
    public record Filter(string value, string property);
    public record Sort(string direction, string timestamp);
    public record SearchPayload(string query = default, Sort sort = default, Filter filter = default, Guid? start_cursor = default, int? page_size = default);
}
