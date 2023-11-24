using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using url_shortener_api.Context;
using url_shortener_api.Models;
using url_shortener_api.utils;

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
		public async Task<ActionResult<User>> Register([FromBody] UserDto newUser)
		{
			Role role = await context.Roles.FirstOrDefaultAsync(x => x.Id == 2);

			if(role == null)
			{
				return BadRequest("Unknow role");
			}

			Mapper mapper = new Mapper();

			User user = mapper.MapUser(newUser, role);

			await context.AddAsync(user);
			await context.SaveChangesAsync();

			return Ok(newUser);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<User>> Login([FromBody] UserDto newUser)
		{
			User user = await context.Users
				.Where(x => x.Name.Equals(newUser.Name) 
				&& x.Password.Equals(newUser.Password))
				.FirstOrDefaultAsync();
	
			if(user == null)
			{
				return BadRequest("Unknow User");
			}

			string jwtToken = Token.CreateToken(user);

			return Ok(jwtToken);
		}

	}
}
