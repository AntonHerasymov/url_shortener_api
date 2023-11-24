using System.ComponentModel.DataAnnotations;

namespace url_shortener_api.Models
{
	public class Role
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
