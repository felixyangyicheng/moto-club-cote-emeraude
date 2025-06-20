namespace Capybara.Models.Configs
{
    public class ApiConfig
    {
        public string Primary { get; set; } = "";
        public List<string> Fallbacks { get; set; } = new();
        public int TimeoutSeconds { get; set; }

        public List<string> AllUris =>
            new[] { Primary }.Concat(Fallbacks).ToList();
    }
}
