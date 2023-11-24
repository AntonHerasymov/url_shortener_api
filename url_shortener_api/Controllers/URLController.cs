using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using url_shortener_api.Context;
using url_shortener_api.Models;

namespace url_shortener_api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class URLController : Controller
	{
		URLContext context;

		public URLController(URLContext context)
		{
			this.context = context;
		}

		[HttpGet("GetURLs")]
		public async Task<ActionResult<URL>> GetAllURLs() 
		{
			var urls = await context.URLs.ToListAsync();

			return Ok(urls);
		}

	}
}
