using Microsoft.AspNetCore.Mvc;
using url_shortener_api.Context;
using url_shortener_api.Models;

namespace url_shortener_api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : Controller
	{
		URLContext context;

		public UserController(URLContext context)
		{
			this.context = context;
		}

	}
}
