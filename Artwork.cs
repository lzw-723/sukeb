using System.Text.Json.Serialization;

namespace sukeb;

public record Artwork(string? Title, string? Artist, string? WorkId, string? FileSize, string? Link, string? ImageLink, uint? Timestamp, string? Description = null);

