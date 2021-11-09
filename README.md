# Notion-SDK
Unnoficial SDK for around Notion API

# Installation

The project is avaiable on [Nuget](https://www.nuget.org/packages/Notion.Sharp/)

```
dotnet add package Notion.Sharp
```

# Usage

The first step for using the SDK is to initialize the client with the Bearer Token and the Notion API Version. 
You should [create a notion integration](https://www.notion.so/my-integrations) to get a bearer token. 

```c#
string bearerToken = BEARER_TOKEN;
string apiVersion = "2021-08-16";
INotion client = Notion.GetClient(bearerToken, apiVersion);
```

## Endpoints

Each notion api endpoint has a corresponding method associated with it
| Name | Method |
|---|---|
| [Query a database](https://developers.notion.com/reference/post-database-query) | `QueryDatabaseAsync` |
| [Create a database](https://developers.notion.com/reference/create-a-database) | `CreateDatabaseAsync` |
| [Update database](https://developers.notion.com/reference/update-a-database) | `UpdateDatabaseAsync` |
| [Retrieve a database](https://developers.notion.com/reference/retrieve-a-database) | `GetDatabaseAsync` |
| [List databases (deprecated)](https://developers.notion.com/reference/get-databases) | `GetDatabasesAsync` |
| [Retrieve a page](https://developers.notion.com/reference/retrieve-a-page) | `GetPageAsync` |
| [Create a page](https://developers.notion.com/reference/post-page) | `CreatePageAsync` |
| [Update page](https://developers.notion.com/reference/patch-page) | `UpdatePageAsync` |
| [Retrieve a page property item](https://developers.notion.com/reference/retrieve-a-page-property) | `GetPagePropertyAsync` |
| [Retrieve a block](https://developers.notion.com/reference/retrieve-a-block) | `GetBlockAsync` |
| [Update a block](https://developers.notion.com/reference/update-a-block) | `UpdateBlockAsync` |
| [Retrieve block children](https://developers.notion.com/reference/get-block-children) | `GetBlocksChildrenAsync` |
| [Append block children](https://developers.notion.com/reference/patch-block-children) | `AppendBlockChildrenAsync` |
| [Delete a block](https://developers.notion.com/reference/delete-a-block) | `DeleteBlockAsync` |
| [Retrieve a user](https://developers.notion.com/reference/get-user) | `GetUserAsync` |
| [List all users](https://developers.notion.com/reference/get-users) | `GetUsersAsync` |
| [Retrieve your token's bot user](https://developers.notion.com/reference/get-self) | `GetMeAsync` |
| [Search](https://developers.notion.com/reference/post-search) | `SearchAsync` |

## Examples

Make a request to any endpoint

```c#
PaginationList<User> users = await client.GetUsersAsync();
```
