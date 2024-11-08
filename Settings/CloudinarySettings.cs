namespace KhontamwebAPI.Settings;

public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
    public string Folder { get; set; } = string.Empty;
    public int MaxWidth { get; set; } = 1300;
    public int RecommendedWidth { get; set; } = 734;
    public int RecommendedHeight { get; set; } = 1104;
} 