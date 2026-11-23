namespace ChestCounter;

/// <summary>Version, GitHub repository and names shared by the tool's processes.</summary>
public static class AppInfo
{
    public static Version Version { get; } = typeof(AppInfo).Assembly.GetName().Version ?? new Version(1, 0, 0);

    public static string VersionText => $"{Version.Major}.{Version.Minor}.{Math.Max(0, Version.Build)}";

    /// <summary>This fork: the exe updates come from its releases.</summary>
    public const string Repo = "jedankomad/Fortnite-PirateTokenChestCounter";
    public static string RepoUrl => "https://github.com/" + Repo;
    public static string IssuesUrl => RepoUrl + "/issues";

    /// <summary>The original project this fork is based on (author: LED-N).</summary>
    public const string UpstreamRepo = "LED-N/Fortnite-ChestCounter";
    public const string UpstreamAuthor = "LED-N";
    public static string UpstreamUrl => "https://github.com/" + UpstreamRepo;
    public const string ForkAuthor = "jedankomad";

    /// <summary>
    /// content.json and the treasure pictures: from the original project, kept up to date by its maintainers every
    /// week (only data, checked by TreasureCatalog.Parse, never code).
    /// </summary>
    public const string ContentRepo = UpstreamRepo;

    /// <summary>Path of ChestCounter.exe when running as the published exe (null under "dotnet ChestCounter.dll").</summary>
    public static string? ExePath
    {
        get
        {
            var p = Environment.ProcessPath;
            return p != null && Path.GetFileName(p).Equals("ChestCounter.exe", StringComparison.OrdinalIgnoreCase) ? p : null;
        }
    }

    // Names of the per-user kernel objects that let the processes find each other.
    /// <summary>One instance per data folder (the normal folder, or a test folder given with --data).</summary>
    public static string InstanceMutexFor(string dataDir)
    {
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(Path.GetFullPath(dataDir).TrimEnd('\\', '/').ToLowerInvariant()));
        return @"Local\ChestCounter.Instance." + Convert.ToHexString(hash, 0, 8);
    }
    /// <summary>Set by a second launch: the instance already open comes to the front.</summary>
    public static string ShowEventFor(string dataDir) => InstanceMutexFor(dataDir).Replace(".Instance.", ".Show.");

    public const string StandbyMutex = @"Local\ChestCounter.Standby";
    public const string StandbyStopEvent = @"Local\ChestCounter.StandbyStop";
}
