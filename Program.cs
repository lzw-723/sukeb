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


IDocument doc = await WebParse.GetDoc("https://skb-nyaa.hacgn.eu.org/?q=aliceholic&f=0&c=2_0");

var tableRows = doc.QuerySelectorAll("tr");

var artworksList = new List<Artwork>();

foreach (var tr in tableRows)
{
    var titleElement = tr.QuerySelector("tr td:nth-child(2) a:nth-last-child(1)");
    // 跳过表头
    if (titleElement == null) continue;
    var title = titleElement!.TextContent.Trim();
    var artist = "";
    // 使用正则表达式查找所有[]内的内容
    MatchCollection items = Regex.Matches(title, @"\[([^\[\]]*)\]");
    if (items.Count > 1)
    {
        Console.WriteLine($"{title}");
        // 获取第一个匹配项
        // title = items[0].Value;
        // title = title.Substring(1, title.Length - 2);
        artist = items[1].Value;
        artist = artist.Substring(1, artist.Length - 2);
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

    artworksList.Add(new Artwork(title, artist, magnet, imageUrl));
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