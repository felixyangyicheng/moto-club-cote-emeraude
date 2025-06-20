

using BlazorTypewriter;
using BootstrapBlazor.Components;
using System.Net.Http;

namespace Capybara.Pages
{
    public partial class Home
    {
        [Inject, NotNull]
        public GraphQLHttpClient GraphQLClient { get; set; } = default!;
        public List<MediaAsset> mediaAssets { get; set; } = new();
        public List<MediaAsset> mediaAssetsToShow { get; set; } = new();


        private string AnimationEntrance = "bounceIn";
        private string AnimationExit = "bounceOut";
        private bool loading { get; set; }
        string _bgCyanLighten1 = $"background:{Colors.Cyan.Lighten1};";
        string _bgWhite = $"background:white;";

        string _turnstileToken = "";
        EventCallback<string> TurnstileCallback;
        EventCallback<string> TurnstileErrorCallback;

        TypewriterBuilder typewriter = new TypewriterBuilder(defaultCharacterPause: 6)
        .TypeString("Un jour, nos routes se croiseront.")
        .Pause(10000)
        .DeleteAll()
        .TypeString(" Un regard, un signe de V, un instant de fraternité sur l'asphalte.", 50)
        .Pause(10000)
        .DeleteAll(30)
        .TypeString(" Peu importe la moto, peu importe la destination, ce qui compte, c'est le voyage et la passion qui nous unit.", 120)
        .Pause(10000)
        .DeleteAll(20)
        .TypeString(" Bonne route et vive la moto ! 🏍️🔥🚀", 50)
        .Pause(10000)
        .DeleteAll(20)
        .Pause(500)
        .Loop();

//        @inject IHttpClientFactory HttpClientFactory



//var client = HttpClientFactory.CreateClient("ApiFallback");
//        var response = await client.GetAsync("endpoint"); // l'uri est géré par les handlers

        protected override async Task OnParametersSetAsync()
        {
            await LoadMedia();
            await base.OnParametersSetAsync();
        }
        private async Task LoadMedia()
        {
            const string query = @"
            query {
              mediaAssets(includeSubDirectories: true) {
                path
                lastModifiedUtc
              }
            }";

            var request = new GraphQLRequest { Query = query };
            //var response = await GraphQLClient.SendQueryAsync<MediaAssetsResponse>(request);
            var response = await GraphQLService.SendQueryWithFallback<MediaAssetsResponse>(request);
         

            if (response.Errors == null)
            {
                mediaAssets = response.Data.MediaAssets;

                foreach (var item in mediaAssets)
                {
                    if (item.Path.Contains(".jpg"))
                    {

                    mediaAssetsToShow.Add(item);
                    }
                    Console.WriteLine($"https://mcce.duckdns.org{item.Path}" );
                }
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
