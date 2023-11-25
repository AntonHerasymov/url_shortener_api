using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
			bool isUser = await context
			.Users
			.AnyAsync(x => x.Login == newUser.Login);

			if (isUser)
			{
				return new ObjectResult(new { message = "Allready Exist" })
				{
					StatusCode = (int)HttpStatusCode.Forbidden, // 403
				};
			}

			Role role = await context.Roles.FirstOrDefaultAsync(x => x.Id == 2);

			if(role == null)
			{
				return BadRequest("Unknow role");
			}

			Mapper mapper = new Mapper();

			User user = mapper.MapUser(newUser, role);

			await context.AddAsync(user);
			await context.SaveChangesAsync();

			return Ok();
		}

		[HttpPost("Login")]
		public async Task<ActionResult<User>> Login([FromBody] UserDto newUser)
		{
			User user = await context.Users
				.Where(x => x.Login.Equals(newUser.Login))
				.FirstOrDefaultAsync();

			if (user == null)
			{
				return new ObjectResult(new { message = "Unknow User" })
				{
					StatusCode = (int)HttpStatusCode.NotFound, // 404
				};
			}


			if (BCrypt.Net.BCrypt.Verify(newUser.Password, user.Password))
			{
				string jwtToken = Token.CreateToken(user);

				return Ok(jwtToken);
			}

			return new ObjectResult(new { message = "Incorrect Login or Password" })
			{
				StatusCode = (int)HttpStatusCode.BadRequest, // 400
			};
		}

	}
}
