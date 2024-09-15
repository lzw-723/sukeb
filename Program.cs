using System.Text.Json.Serialization;
using AngleSharp;
using AngleSharp.Dom;
using sukeb;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();


IDocument doc = await WebParse.GetDoc("https://skb-nyaa.hacgn.eu.org/?q=aliceholic&f=0&c=2_0");

var tableRows = doc.QuerySelectorAll("tr");

var artworksList = new List<Artwork>();

Console.WriteLine(tableRows.Length);
foreach (var tr in tableRows)
{
    var titleElement = tr.QuerySelector("tr td:nth-child(2) a:nth-last-child(1)");
    // 跳过表头
    if (titleElement == null) continue;
    var title = titleElement!.TextContent.Trim();
    var magnetElement = tr.QuerySelector("tr td:nth-child(3) a:nth-child(2)");
    var magnet = magnetElement!.GetAttribute("href")!.Trim();

    artworksList.Add(new Artwork(title, "artist", magnet, "imageLink"));
    Console.WriteLine($"{title} {magnet}");
}

var api = app.MapGroup("/api/v1");
var todosApi = api.MapGroup("/artworks");
todosApi.MapGet("/", () => artworksList);
todosApi.MapGet("/{id}", (int id) =>
    artworksList.FirstOrDefault(a => true) is { } artwork
        ? Results.Ok(artwork)
        : Results.NotFound());

// 静态资源
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();

[JsonSerializable(typeof(List<Artwork>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}