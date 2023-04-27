using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notion.experiment;
using Notion.Model;

using Refit;

namespace Notion;

[Headers("Authorization: Bearer")]
public interface INotion
{
    #region Databases

    /// <summary>
    /// Creates a database as a subpage in the specified parent page, with the specified properties schema.
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    [Post("/databases")]
    Task<Database> CreateDatabaseAsync(Database database);

    [Patch("/databases/{id}")]
    Task<Database> UpdateDatabaseAsync(Guid id, [Body] object database);

    /// <summary>
    /// Updates an existing database as specified by the parameters.
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    public async Task<Database> UpdateDatabaseAsync(Database database)
    {
        return await UpdateDatabaseAsync(database.Id, new
        {
            title = database.Title,
            properties = database.Properties,
        });
    }

    /// <summary>
    /// List all Databases shared with the authenticated integration.
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="startCursor"></param>
    /// <remarks>This endpoint is no longer recommended, use search instead. 
    /// This endpoint will only return explicitly shared pages, while search will also return child pages within explicitly shared pages. 
    /// This endpoint's results cannot be filtered, while search can be used to match on page title.</remarks>
    /// <returns></returns>
    [Obsolete("Don't use this endpoint")]
    [Get("/databases")]
    Task<PaginationList<Database>> GetDatabasesAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

    /// <summary>
    /// Retrieves a Database object using the ID specified.
    /// </summary>
    /// <param name="databaseId"></param>
    /// <returns></returns>
    [Get("/databases/{database_id}")]
    Task<Database> GetDatabaseAsync([AliasAs("database_id")] Guid databaseId);

    /// <summary>
    /// Retrieves a Database Json using the specified Id
    /// </summary>
    /// <param name="databaseId">The id of the database.</param>
    /// <returns>The json string representing the Database</returns>
    [Get("/databases/{database_id}")]
    Task<string> GetDatabaseRawAsync([AliasAs("database_id")] Guid databaseId);

    /// <summary>
    /// Gets a list of Pages contained in the database, filtered and ordered according to the filter conditions and sort criteria provided in the request. 
    /// The response may contain fewer than page_size of results.
    /// </summary>
    /// <param name="id">Identifier for a Notion database.</param>
    /// <param name="searchPayload"></param>
    /// <returns></returns>
    [Post("/databases/{id}/query")]
    Task<PaginationList<Page>> QueryDatabaseAsync(Guid id, [Body] object searchPayload);

    #endregion

    #region Pages

    /// <summary>
    /// Retrieves a Page object using the ID specified.
    /// </summary>
    /// <param name="pageId"></param>
    /// <returns></returns>
    [Get("/pages/{page_id}")]
    Task<Page> GetPageAsync([AliasAs("page_id")] Guid pageId);

    /// <summary>
    /// Retrieves a Page object using the Id specified.
    /// </summary>
    /// <param name="pageId">The id of the page to be retrieved.</param>
    /// <returns>A json string representation of the page.</returns>
    [Get("/pages/{page_id}")]
    Task<string> GetPageRawAsync([AliasAs("page_id")] Guid pageId);

    /// <summary>
    /// Updates page property values for the specified page. Properties that are not set via the properties parameter will remain unchanged.
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    [Patch("/pages/{page.id}")]
    Task<Page> UpdatePageAsync(Page page);

    [Post("/pages")]
    internal Task<Page> CreatePageAsync([Body] object page);

    /// <summary>
    /// Creates a new page in the specified database or as a child of an existing page.
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<Page> CreatePageAsync(Page page)
    {
        return await CreatePageAsync(page as object);
    }

    /// <inheritdoc cref="CreatePageAsync(Page)"/>
    public async Task<Page> CreatePageAsync(Page page, Block[] children)
    {
        return await CreatePageAsync(new
        {
            parent = page.Parent,
            properties = page.Properties,
            children,
        });
    }

    #endregion

    #region Blocks

    /// <summary>
    /// Retrieves a Block object using the ID specified.
    /// </summary>
    /// <param name="blockId"></param>
    /// <returns></returns>
    [Get("/blocks/{block_id}")]
    Task<Block> GetBlockAsync([AliasAs("block_id")] Guid blockId);
    
    /// <summary>
    /// Retrieves a Block object using the ID specified.
    /// </summary>
    /// <param name="blockId">The id of the block to get.</param>
    /// <returns>The raw json representing the block.</returns>
    [Get("/blocks/{block_id}")]
    Task<string> GetBlockRawAsync([AliasAs("block_id")] Guid blockId);

    /// <summary>
    /// Updates the content for the specified block_id based on the block type. 
    /// Supported fields based on the block object type. 
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    [Patch("/blocks/{block.Id}")]
    Task<Block> UpdateBlockAsync(Block block);
    

    /// <summary>
    /// Returns a paginated array of child block objects contained in the block using the ID specified. 
    /// In order to receive a complete representation of a block, you may need to recursively retrieve the block children of child blocks. 
    /// The response may contain fewer than page_size of results.
    /// </summary>
    /// <param name="blockId"></param>
    /// <param name="pageSize"></param>
    /// <param name="startCursor"></param>
    /// <returns></returns>
    [Get("/blocks/{id}/children")]
    Task<PaginationList<Block>> GetBlocksChildrenAsync([AliasAs("id")] Guid blockId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

    [Patch("/blocks/{id}/children")]
    internal Task<PaginationList<Block>> AppendBlockChildrenAsync(Guid id, [Body] object content);

    /// <summary>
    /// Creates and appends new children blocks to the parent block_id specified. 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<PaginationList<Block>> AppendBlockChildrenAsync(Guid id, List<Block> content)
    {
        return await AppendBlockChildrenAsync(id, new
        {
            children = content
        });
    }

    /// <summary>
    /// Sets a Block object, including page blocks, to archived: true using the ID specified. 
    /// Note: in the Notion UI application, this moves the block to the "Trash" where it can still be accessed and restored. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Delete("/blocks/{id}")]
    Task<Block> DeleteBlockAsync(Guid id);

    #endregion

    #region Users

    [Get("/users")]
    Task<PaginationList<User>> GetUsersAsync([AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

    /// <summary>
    /// Retrieves a User using the ID specified.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [Get("/users/{user_id}")]
    Task<User> GetUserAsync([AliasAs("user_id")] Guid userId);

    /// <summary>
    /// Retrieves the bot User associated with the API token provided in the authorization header. 
    /// The bot will have an owner field with information about the person who authorized the integration.
    /// </summary>
    /// <returns></returns>
    [Get("/users/me")]
    Task<User> GetMeAsync();

    #endregion

    #region Search

    /// <summary>
    /// Searches all pages and child pages that are shared with the integration. The results may include databases.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [Post("/search")]
    [Headers("Content-Type: application/json")]
    Task<PaginationList<PageOrDatabase>> SearchAsync([Body] SearchPayload query);
    
    [Get("/pages/{pageId}/properties/{propertyId}")]
    [Headers("Content-Type: application/json")]
    Task<Foo> GetPagePropertyAsync(Guid pageId, string propertyId, [AliasAs("page_size")] int pageSize = 100, [AliasAs("start_cursor")] Guid? startCursor = default);

    #endregion

}

