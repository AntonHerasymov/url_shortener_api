using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
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

		[HttpGet("GetUrls")]
		public async Task<ActionResult<URL>> GetAllURLs()
		{
			var urls = await context.URLs
				.Include(x => x.CreatedBy)
				.Select(url => new
				{
					id = url.Id,
					shortUrl = url.ShortUrl,
					createdByName = url.CreatedBy.Login
				})
				.ToListAsync();

			return Ok(urls);
		}

		[HttpGet("GetUrl")]
		public async Task<ActionResult<URL>> GetUrlById(int urlId)
		{
			var url = await context.URLs
				.Where(x => x.Id == urlId)
				.Include(x => x.CreatedBy)
				.Select(url => new
				{
					id = url.Id,
					fullUrl = url.FullUrl,
					shortUrl = url.ShortUrl,
					createdByName = url.CreatedBy.Login,
					createdDate = url.CreatedDate,
				})
				.FirstOrDefaultAsync();

			return Ok(url);
		}

		[HttpPost("AddNew")]
		public async Task<ActionResult<URL>> AddNew([FromBody] URLDto newURL)
		{
			if (!Uri.TryCreate(newURL.fullUrl, UriKind.Absolute, out var url_))
			{
					return new ObjectResult(new { message = "Not a Url" })
					{
						StatusCode = (int)HttpStatusCode.BadRequest, // 400
					};
			}

			User user = await context
				.Users
				.FirstOrDefaultAsync(x => x.Id == newURL.userId);

			if (user == null)
			{
				return new ObjectResult(new { message = "Unauthorized" })
				{
					StatusCode = (int)HttpStatusCode.Unauthorized, // 401
				};
			}

			bool isUrl = context.URLs.Any(x => x.FullUrl == newURL.fullUrl);

			if (isUrl)
			{
				return new ObjectResult(new { message = "Already Exist" })
				{
					StatusCode = (int)HttpStatusCode.Forbidden // 403
				};

			}

			URL shortenedUrl = await urlShorteningService.GenerateShortUrl(newURL, user);

			await context.URLs.AddAsync(shortenedUrl);
			await context.SaveChangesAsync();

			var urlToSend = new
			{
				id = shortenedUrl.Id,
				shortUrl = shortenedUrl.ShortUrl,
				createdByName = shortenedUrl.CreatedBy.Login
			};

			return CreatedAtAction(nameof(AddNew), new { id = shortenedUrl.Id }, urlToSend);
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
