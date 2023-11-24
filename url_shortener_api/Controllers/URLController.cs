using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using url_shortener_api.Context;
using url_shortener_api.Models;
using url_shortener_api.Service;
using url_shortener_api.utils;

namespace url_shortener_api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class URLController : Controller
	{
		private readonly URLContext context;
		private readonly UrlShorteningService urlShorteningService;

		public URLController
			(
			URLContext context,
			UrlShorteningService urlShorteningService
			)
		{
			this.context = context;
			this.urlShorteningService = urlShorteningService;
		}

		[HttpGet("GetURLs")]
		public async Task<ActionResult<URL>> GetAllURLs()
		{
			var urls = await context.URLs.Include(x => x.CreatedBy).ThenInclude(x => x.Role).ToListAsync();

			return Ok(urls);
		}

		[Authorize]
		[HttpPost("AddNew")]
		public async Task<ActionResult<URL>> AddNew([FromBody] URLDto newURL)
		{
			User user = await context
				.Users
				.FirstOrDefaultAsync(x => x.Id == newURL.UserId);

			if (user == null)
			{
				return BadRequest("Unknow User");
			}

			Mapper mapper = new Mapper();

			string code = await URLShorter.shorten(newURL.FullUrl, urlShorteningService);

			Uri uri = new Uri(newURL.FullUrl);

			string scheme = uri.Scheme;
			string host = uri.Host;

			string shortUrl = $"{scheme}://{host}/api/{code}";

			newURL.ShortUrl = shortUrl;


			URL shortenedUrl = mapper.MapUrl(newURL, user, code);

			await context.URLs.AddAsync(shortenedUrl);
			await context.SaveChangesAsync();

			return Ok(shortenedUrl);
		}

		[HttpDelete("Delete")]
		public async Task<ActionResult> Delete([FromBody] int urlId)
		{
			URL url = await context.URLs.FirstOrDefaultAsync(x => x.Id == urlId);

			if(url == null)
			{
				return BadRequest("Unknow URl");
			}

			context.Remove(url);
			await context.SaveChangesAsync();

			return Ok();
		}
	}
}
