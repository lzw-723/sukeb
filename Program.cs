using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using sukeb;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var artworksList = new List<Artwork>();

FetchNyaa(artworksList);

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

async Task FetchNyaa(List<Artwork> artworks)
{
    IDocument doc = await WebParse.GetDoc("https://skb-nyaa.hacgn.eu.org/?q=aliceholic&f=0&c=2_0");
    var tableRows = doc.QuerySelectorAll("tr");

    foreach (var tr in tableRows)
    {
        var titleElement = tr.QuerySelector("tr td:nth-child(2) a:nth-last-child(1)");
        // 跳过表头
        if (titleElement == null) continue;
        var title = titleElement!.TextContent.Trim();
        var artist = "";
        var workId = "";
        // 使用正则表达式查找所有[]内的内容
        MatchCollection items = Regex.Matches(title, @"\[([^\[\]]*)\]");
        foreach (Match item in items)
        {
            var t = item.Value;
            t = t.Substring(1, t.Length - 2);
            
            if (t.Contains("Alice"))
            {
                artist = t;
            }
            else if (t.StartsWith("fantia"))
            {
                workId = t;
            }
        }

        var link = titleElement!.GetAttribute("href")!.Trim();

        var view = await WebParse.GetDoc("https://skb-nyaa.hacgn.eu.org" + link);
        var desc = view.QuerySelector("#torrent-description")!.InnerHtml;

        string imageUrl = "";
        // 使用正则表达式查找第一个图片的URL  
        string pattern = @"!\[[^\]]*\]\(([^)]*)\)";
        Match match = Regex.Match(desc, pattern);
        if (match.Success)
        {
            // match.Groups[1] 包含了第一个捕获组的内容，即图片的URL  
            imageUrl = match.Groups[1].Value;
        }

        var magnetElement = tr.QuerySelector("tr td:nth-child(3) a:nth-child(2)");
        var magnet = magnetElement!.GetAttribute("href")!.Trim();

        artworks.Add(new Artwork(title, artist, workId,magnet, imageUrl));
        app.Logger.LogInformation("Found artwork: {title}", title);
    }

    app.Logger.LogInformation("Nyaa created");
}

[JsonSerializable(typeof(List<Artwork>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}