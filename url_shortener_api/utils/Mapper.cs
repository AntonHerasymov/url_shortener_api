using url_shortener_api.Models;

namespace url_shortener_api.utils
{
	public class Mapper
	{
		public URL MapUrl(URLDto urlDto, User user, string code)
		{
			return new URL
			{
				Id = urlDto.id,
				FullUrl = urlDto.fullUrl,
				ShortUrl = urlDto.shortUrl,
				Code = code,
				CreatedBy = user,
				CreatedDate = DateTime.Now,
			};
		}

		public User MapUser(UserDto userDto,Role role)
		{
			return new User
			{
				Id = userDto.Id,
				Login = userDto.Login,
				Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
				Role = role,
			};
		}

	}
}
