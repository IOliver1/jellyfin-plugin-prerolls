using Jellyfin.Plugin.Prerolls.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Prerolls;

/// <summary>
/// Adult Swim Pre-rolls plugin — plays a random bump before movies and/or TV episodes.
/// Targets Jellyfin 10.11.x (net9.0).
/// </summary>
public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    /// </summary>
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <inheritdoc />
    public override string Name => "Prerolls";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("4b5b2c4a-d3f1-4e2b-9c8e-1a2b3c4d5e6f");

    /// <inheritdoc />
    public override string Description => "Plays a random pre-roll video before movies and/or TV episodes. Perfect for Adult Swim bumps.";

    /// <summary>Gets the singleton instance.</summary>
    public static Plugin? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.configPage.html",
            },
        ];
    }
}
