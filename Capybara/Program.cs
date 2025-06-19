
using Capybara.HashCheckService;
using Capybara.Providers;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Components.Authorization;
using Polly;
using Polly.Timeout;
using System.IdentityModel.Tokens.Jwt;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


//builder.Services.AddHttpClient("ApiFallback", client =>
//    client.BaseAddress = new Uri(builder.Configuration["ApiUris:Primary"]))
//.AddPolicyHandler((sp, request) =>
//{
//    // Politique Timeout de 25 s
//    var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(25));

//    // Politique Retry + Fallback sur d'autres URL
//    var uris = sp.GetRequiredService<IConfiguration>()
//                 .GetSection("ApiUris:Fallbacks").Get<string[]>();

//    var retryAndFallback = Policy<HttpResponseMessage>
//        .Handle<TimeoutRejectedException>()
//        .Or<HttpRequestException>()
//        .RetryAsync(uris.Length, onRetryAsync: async (outcome, retryCount, context) =>
//        {
//            context["FallbackUri"] = uris[retryCount - 1];
//        })
//        .WrapAsync(
//            Policy<HttpResponseMessage>
//              .Handle<TimeoutRejectedException>()
//              .Or<HttpRequestException>()
//              .FallbackAsync(
//                  fallbackAction: (ctx, ct) =>
//                  {
//                      var fb = ctx["FallbackUri"] as string;
//                      var client = sp.GetRequiredService<IHttpClientFactory>()
//                                     .CreateClient("ApiFallback");
//                      return client.GetAsync(fb, ct);
//                  }
//              )
//        );

//    return timeoutPolicy.WrapAsync(retryAndFallback);
//});



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build());
#if DEBUG
builder.Services.AddHttpClient("notification.push.srv.local", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("notification.push.srv.local") ?? throw new ArgumentException());
    });

builder.Services.AddScoped(sp => new GraphQLHttpClient(
    new GraphQLHttpClientOptions
    {
        EndPoint = new Uri(builder.Configuration.GetValue<string>("graphsql.api.local") ?? throw new ArgumentException()),
    },
    new SystemTextJsonSerializer(),
    sp.GetRequiredService<IHttpClientFactory>().CreateClient()));
#else
    builder.Services.AddHttpClient("notification.push.srv", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("notification.push.srv") ?? throw new ArgumentException());
    });
    builder.Services.AddScoped(sp => new GraphQLHttpClient(
    new GraphQLHttpClientOptions
    {
        EndPoint = new Uri(builder.Configuration.GetValue<string>("graphsql.api") ?? throw new ArgumentException()),
    },
    new SystemTextJsonSerializer(),
    sp.GetRequiredService<IHttpClientFactory>().CreateClient()));
#endif

builder.Services.AddAuthorizationCore();
builder.Services.AddBootstrapBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<HashServiceFactory>();
builder.Services.AddScoped<SiteMapService>();

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddScoped<JwtSecurityTokenHandler>();

builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 2000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

await builder.Build().RunAsync();