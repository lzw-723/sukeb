using AngleSharp;
using AngleSharp.Dom;
using IConfiguration = AngleSharp.IConfiguration;

namespace sukeb;

public class WebParse
{
    public static async Task<IDocument> GetDoc(String address)
    {
        IConfiguration config = Configuration.Default.WithDefaultLoader();
        IBrowsingContext context = BrowsingContext.New(config);
        IDocument document = await context.OpenAsync(address);
        return document;
    }
    
}