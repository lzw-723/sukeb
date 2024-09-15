using System.Text.Json.Serialization;

namespace sukeb;

public record Artwork(String? Title, String? Artist, String? Link, String? ImageLink = null, String? Description = null);

