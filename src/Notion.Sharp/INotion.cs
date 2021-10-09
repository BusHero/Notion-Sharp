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
        Task<string> CreateDatabaseAsync([Body] object database);

        [Patch("/databases/{id}")]
        Task<string> UpdateDatabaseAsync(Guid id, [Body]object database);
        
        [Get("/databases")]
        Task<string> GetDatabasesAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/databases/{database_id}")]
        Task<string> GetDatabaseAsync([AliasAs("database_id")] Guid databaseId);

        #endregion

        #region Users

        [Get("/users")]
        Task<PaginationList<object>> GetUsersAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Get("/users/{user_id}")]
        Task<string> GetUserAsync([AliasAs("user_id")] Guid userId);

        [Get("/users/me")]
        Task<string> GetMeAsync();

        #endregion

        #region Pages

        [Get("/pages/{page_id}")]
        Task<string> GetPageAsync([AliasAs("page_id")] Guid pageId);

        [Patch("/pages/{id}")]
        Task<string> UpdatePageAsync(Guid id, [Body] object page);

        [Post("/pages")]
        Task<string> CreatePageAsync([Body]object page);

        #endregion

        #region Blocks

        [Get("/blocks/{block_id}")]
        Task<string> GetBlockAsync([AliasAs("block_id")] Guid blockId);
        
        [Patch("/blocks/{id}")]
        Task<string> UpdateBlockAsync(Guid id, [Body] object block);

        [Get("/blocks/{id}/children")]
        Task<PaginationList<object>> GetBlocksChildrenAsync([AliasAs("id")] Guid blockId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

        [Patch("/blocks/{id}/children")]
        Task<PaginationList<object>> AppendBlockChildrenAsync(Guid id, [Body] object content);
        
        [Delete("/blocks/{id}")]
        Task<string> DeleteBlockAsync(Guid id);

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
