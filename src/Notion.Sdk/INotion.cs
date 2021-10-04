using System;
using System.Threading.Tasks;

using Notion.Model;

using Refit;

namespace Notion
{
    [Headers("Notion-Version: 2021-05-13", "Authorization: Bearer")]
    public interface INotion
    {
        #region Databases

        [Post("/databases")]
        Task<ApiResponse<Database>> CreateDatabaseAsync(Database database);

        [Patch("/databases/{database.id}")]
        Task<ApiResponse<Database>> UpdateDatabaseAsync(Database database);
        
        [Get("/databases")]
        Task<ApiResponse<List<Database>>> GetDatabasesAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/databases/{database_id}")]
        Task<ApiResponse<Database>> GetDatabaseAsync([AliasAs("database_id")] Guid databaseId);

        #endregion

        #region Users

        [Get("/users")]
        Task<ApiResponse<List<User>>> GetUsersAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/users/{user_id}")]
        Task<ApiResponse<User>> GetUserAsync([AliasAs("user_id")] Guid userId);

        #endregion

        #region Pages

        [Get("/pages/{page_id}")]
        Task<ApiResponse<Page>> GetPageAsync([AliasAs("page_id")] Guid pageId);

        [Patch("/pages/{page.id}")]
        Task<ApiResponse<Page>> UpdatePageAsync(Page page);

        [Post("/pages")]
        Task<ApiResponse<Page>> CreatePageAsync(Page page);

        #endregion

        #region Blocks

        [Get("/blocks/{id}/children")]
        Task<ApiResponse<List<Block>>> GetBlocksChildrenAsync([AliasAs("id")] Guid blockId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/blocks/{block_id}")]
        Task<ApiResponse<Block>> GetBlock([AliasAs("block_id")] Guid blockId);

        [Patch("/blocks/{block.id}")]
        Task<ApiResponse<Block>> UpdateBlockAsync(Block block);

        [Patch("/blocks/{id}/children")]
        Task<ApiResponse<List<Block>>> AddChildrenToBlock(Guid id, [Body] object content);

        #endregion
    }
}
