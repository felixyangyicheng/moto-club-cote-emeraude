namespace Capybara.Models.Configs;

public class ApiConfig
{
    public string Primary { get; set; } = string.Empty;
    public List<string> Fallbacks { get; set; } = new List<string>();

    private int _timeoutSeconds = 30;
    public int TimeoutSeconds
    {
        get => _timeoutSeconds;
        set => _timeoutSeconds = value > 0 ? value : 30;
    }

    public List<string> AllUris => new[] { Primary }.Concat(Fallbacks).ToList();

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Primary))
            throw new InvalidOperationException("Primary API URL is not configured");

        if (TimeoutSeconds <= 0)
            throw new InvalidOperationException("Timeout must be positive");

        if (Fallbacks == null || !Fallbacks.Any())
            throw new InvalidOperationException("At least one fallback URL is required");

        foreach (var url in AllUris)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                throw new InvalidOperationException($"Invalid URL format: {url}");
        }
    }
}