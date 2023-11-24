using System.ComponentModel.DataAnnotations;

namespace url_shortener_api.Models
{
	public class URL
	{
		[Key]
		public int id { get; set; }

		public string full_url { get; set; }

		public string short_url { get; set; }

		public User created_by { get; set; }

		public DateTime created_date { get; set; }

	}
}
