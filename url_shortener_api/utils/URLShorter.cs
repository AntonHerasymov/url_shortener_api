using url_shortener_api.Service;

namespace url_shortener_api.utils
{
	public class URLShorter
	{

		public async static Task<string> shorten(string Url,UrlShorteningService urlShorteningService) 
		{
			if(!Uri.TryCreate(Url, UriKind.Absolute, out var url_))
			{
				throw new ArgumentException("Invalid URL");
			}

			var code = await urlShorteningService.GenerateUniqueCode();

			return code;
		}
	}
}
