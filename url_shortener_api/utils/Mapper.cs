using url_shortener_api.Models;

namespace url_shortener_api.utils
{
	public class Mapper
	{
		public URL Map(URLDto urlDto, User user, string code)
		{
			return new URL
			{
				Id = urlDto.Id,
				FullUrl = urlDto.FullUrl,
				ShortUrl = urlDto.ShortUrl,
				Code = code,
				CreatedBy = user,
				CreatedDate = urlDto.CreatedDate,
			};
		}
		
	}
}
