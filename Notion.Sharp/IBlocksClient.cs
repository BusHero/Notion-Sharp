using System.Threading;
using System.Threading.Tasks;
using Notion.Model;

namespace Notion;

public interface IBlocksClient
{
    Task<Block> Get(CancellationToken cancellationToken = default);
}