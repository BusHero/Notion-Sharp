using Microsoft.Extensions.Configuration;using Notion;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
    
var token = configuration["NotionClient"] ?? throw new InvalidOperationException();

var client = new NotionClient(new Credentials(token));
var page = await client.Page(Guid.Parse("ac84310baefa4282818a56211c07368e")).Get();
foreach (var property in page!.Properties!.Keys)
{
    Console.WriteLine(page.Properties[property]);
}