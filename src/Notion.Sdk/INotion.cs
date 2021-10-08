using System;
using System.Threading.Tasks;

using Notion.Model;

using Refit;

namespace Notion
{
    [Headers("Notion-Version: 2021-08-16", "Authorization: Bearer")]
    public interface INotion
    {
        #region Databases

        [Post("/databases")]
        Task<string> CreateDatabaseAsync(Database database);

        [Patch("/databases/{database.id}")]
        Task<string> UpdateDatabaseAsync(Database database);
        
        [Get("/databases")]
        Task<string> GetDatabasesAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/databases/{database_id}")]
        Task<string> GetDatabaseAsync([AliasAs("database_id")] Guid databaseId);

        #endregion

        #region Users

        [Get("/users")]
        Task<string> GetUsersAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/users/{user_id}")]
        Task<string> GetUserAsync([AliasAs("user_id")] Guid userId);

        [Get("/users/me")]
        Task<string> GetMeAsync();

        #endregion

        #region Pages

        [Get("/pages/{page_id}")]
        Task<string> GetPageAsync([AliasAs("page_id")] Guid pageId);

        [Patch("/pages/{page.id}")]
        Task<string> UpdatePageAsync(Page page);

        [Post("/pages")]
        Task<string> CreatePageAsync(Page page);

        #endregion

        #region Blocks

        [Get("/blocks/{id}/children")]
        Task<string> GetBlocksChildrenAsync([AliasAs("id")] Guid blockId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/blocks/{block_id}")]
        Task<string> GetBlock([AliasAs("block_id")] Guid blockId);

        [Patch("/blocks/{block.id}")]
        Task<string> UpdateBlockAsync(Block block);

        [Patch("/blocks/{id}/children")]
        Task<string> AddChildrenToBlock(Guid id, [Body] object content);

        #endregion

        #region Search
        [Post("/search")]
        [Headers("Content-Type: application/json")]
        Task<string> SearchAsync([Body]SearchPayload query);

        #endregion
    }
}
