using System.Threading;
using System.Threading.Tasks;
using Notion.Model;

namespace Notion;

public interface IPagesClient
{
    public Task<Page> Get(CancellationToken cancellationToken = default);
}