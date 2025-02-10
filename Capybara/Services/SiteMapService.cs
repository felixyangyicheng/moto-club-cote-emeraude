using System.Text;

namespace Capybara.Services
{
	public class SiteMapService
	{

		private readonly NavigationManager _navigationManager;

		public SiteMapService(NavigationManager navigationManager)
		{
			_navigationManager = navigationManager;
		}

		public string GenerateSitemap()
		{
			var baseUrl = _navigationManager.BaseUri.TrimEnd('/');

			var urls = new List<string>
		{
			$"{baseUrl}/",
			$"{baseUrl}/about",
			$"{baseUrl}/contact",
			$"{baseUrl}/events",
			$"{baseUrl}/history",
			$"{baseUrl}/legal",
			$"{baseUrl}/news",
			$"{baseUrl}/presentation",
			$"{baseUrl}/siteplan"
		};

			var sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

			foreach (var url in urls)
			{
				sb.AppendLine("  <url>");
				sb.AppendLine($"    <loc>{url}</loc>");
				sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
				sb.AppendLine("    <priority>0.8</priority>");
				sb.AppendLine("  </url>");
			}

			sb.AppendLine("</urlset>");

			return sb.ToString();
		}
	}
}
