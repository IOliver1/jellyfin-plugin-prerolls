namespace Jellyfin.Plugin.Prerolls.Configuration;

/// <summary>
/// Plugin configuration saved to disk.
/// </summary>
public class PluginConfiguration : MediaBrowser.Model.Plugins.BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the local folder containing pre-roll .mp4 files.
    /// </summary>
    public string PrerollFolder { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets which media types trigger a pre-roll.
    /// </summary>
    public PrerollMediaType MediaType { get; set; } = PrerollMediaType.Both;
}

/// <summary>
/// Which media types will have a pre-roll injected.
/// </summary>
public enum PrerollMediaType
{
    /// <summary>Movies only.</summary>
    Movies = 0,

    /// <summary>TV episodes only.</summary>
    TvShows = 1,

    /// <summary>Both movies and TV episodes.</summary>
    Both = 2,
}
