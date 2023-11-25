using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace url_shortener_api.Models
{
	public class URL
	{
		[Key]
		public int Id { get; set; }

		public string FullUrl { get; set; } = string.Empty;

		public string ShortUrl { get; set; } = string.Empty;

		
		public string Code {  get; set; } = string.Empty;

		public User CreatedBy { get; set; } = new User();
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}

	public class URLDto
	{
		public int id { get; set; }
		public string fullUrl { get; set; } = string.Empty;
		public string shortUrl { get; set; } = string.Empty;
		public int userId { get; set; }
		public DateTime createdDate { get; set; }
	}
}
