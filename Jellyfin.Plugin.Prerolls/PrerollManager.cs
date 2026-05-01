using Jellyfin.Database.Implementations.Entities;
using Jellyfin.Plugin.Prerolls.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Prerolls;

/// <summary>
/// Provides a randomly-selected pre-roll video before movies and/or TV episodes.
/// Implements <see cref="IIntroProvider"/> so Jellyfin injects it automatically
/// at playback time — no Cinema Mode required.
/// </summary>
public class PrerollManager : IIntroProvider
{
    private static readonly string[] VideoExtensions =
        [".mp4", ".mkv", ".avi", ".mov", ".wmv", ".m4v", ".webm"];

    private readonly ILogger<PrerollManager> _logger;
    private readonly Random _random = new();

    /// <summary>
    /// Initializes a new instance of <see cref="PrerollManager"/>.
    /// </summary>
    public PrerollManager(ILogger<PrerollManager> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public string Name => "Prerolls";

    /// <inheritdoc />
    public Task<IEnumerable<IntroInfo>> GetIntros(BaseItem item, User user)
    {
        var config = Plugin.Instance?.Configuration ?? new PluginConfiguration();
        var folder = config.PrerollFolder;

        // ── 1. Guard: folder must be configured and exist ──────────────────
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
        {
            _logger.LogDebug("[Prerolls] Folder not configured or missing: {Folder}", folder);
            return Task.FromResult(Enumerable.Empty<IntroInfo>());
        }

        // ── 2. Guard: only fire for the configured media type ──────────────
        var mediaType = config.MediaType;
        bool isMovie   = item is Movie;
        bool isEpisode = item is Episode;

        bool shouldRun = mediaType switch
        {
            PrerollMediaType.Movies  => isMovie,
            PrerollMediaType.TvShows => isEpisode,
            PrerollMediaType.Both    => isMovie || isEpisode,
            _                        => false,
        };

        if (!shouldRun)
        {
            _logger.LogDebug("[Prerolls] Skipping {ItemType} '{Name}' (MediaType setting: {Setting})",
                item.GetType().Name, item.Name, mediaType);
            return Task.FromResult(Enumerable.Empty<IntroInfo>());
        }

        // ── 3. Pick a random video file from the folder ────────────────────
        var videos = Directory
            .EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly)
            .Where(f => VideoExtensions.Contains(
                Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
            .ToArray();

        if (videos.Length == 0)
        {
            _logger.LogWarning("[Prerolls] No video files found in {Folder}", folder);
            return Task.FromResult(Enumerable.Empty<IntroInfo>());
        }

        var pick = videos[_random.Next(videos.Length)];
        _logger.LogInformation("[Prerolls] Injecting '{Pick}' before '{Name}'", pick, item.Name);

        return Task.FromResult<IEnumerable<IntroInfo>>(
        [
            new IntroInfo { Path = pick }
        ]);
    }
}
