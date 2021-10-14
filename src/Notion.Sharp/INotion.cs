using System;
using System.Threading.Tasks;

using Notion.Model;

using Refit;

namespace Notion
{
    [Headers("Authorization: Bearer")]
    public interface INotion
    {
        #region Databases

        [Post("/databases")]
        Task<Database> CreateDatabaseAsync([Body] object database);

        [Patch("/databases/{id}")]
        Task<Database> UpdateDatabaseAsync(Guid id, [Body]object database);
        
        [Obsolete]
        [Get("/databases")]
        Task<string> GetDatabasesAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/databases/{database_id}")]
        Task<Database> GetDatabaseAsync([AliasAs("database_id")] Guid databaseId);

        #endregion

        #region Users

        [Get("/users")]
        Task<PaginationList<User>> GetUsersAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/users/{user_id}")]
        Task<User> GetUserAsync([AliasAs("user_id")] Guid userId);

        [Get("/users/me")]
        Task<User> GetMeAsync();

        #endregion

        #region Pages

        [Get("/pages/{page_id}")]
        Task<Page> GetPageAsync([AliasAs("page_id")] Guid pageId);

        [Patch("/pages/{id}")]
        Task<Page> UpdatePageAsync(Guid id, [Body] object page);

        [Post("/pages")]
        Task<Page> CreatePageAsync([Body]object page);

        #endregion

        #region Blocks

        [Get("/blocks/{block_id}")]
        Task<Block> GetBlockAsync([AliasAs("block_id")] Guid blockId);
        
        [Patch("/blocks/{id}")]
        Task<Block> UpdateBlockAsync(Guid id, [Body] object block);

        [Get("/blocks/{id}/children")]
        Task<PaginationList<Block>> GetBlocksChildrenAsync([AliasAs("id")] Guid blockId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Patch("/blocks/{id}/children")]
        Task<PaginationList<Block>> AppendBlockChildrenAsync(Guid id, [Body] object content);
        
        [Delete("/blocks/{id}")]
        Task<Block> DeleteBlockAsync(Guid id);

        #endregion

        #region Search
        [Post("/search")]
        [Headers("Content-Type: application/json")]
        Task<PaginationList<object>> SearchAsync([Body]SearchPayload query);
        
        [Post("/databases/{id}/query")]
        Task<PaginationList<object>> QueryDatabaseAsync(Guid id, [Body]object p);
        
        
        #endregion
    }
}
