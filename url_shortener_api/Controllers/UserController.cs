using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		[HttpPost("Register")]
		public async Task<ActionResult<User>> Register([FromBody] User newUser)
		{
			newUser.Role = await context.Roles.FirstOrDefaultAsync(x => x.Id == 2);

			object value = await context.Users.AddAsync(newUser);

			await context.SaveChangesAsync();

			return Ok(newUser);
		}

	}
}
