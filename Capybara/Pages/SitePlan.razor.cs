namespace Capybara.Pages
{
	public partial class SitePlan
	{
		[Inject, NotNull] protected IJSRuntime? JS { get; set; }
		[Inject, NotNull] protected SiteMapService? SiteMapService  { get; set; }
		private string sitemapContent = "";

		protected override void OnInitialized()
		{
			sitemapContent = SiteMapService.GenerateSitemap();
		}
		private async Task DownloadSitemap()
		{
			await JS.InvokeVoidAsync("downloadSitemap", sitemapContent);
		}
	}
}
