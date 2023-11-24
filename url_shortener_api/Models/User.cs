using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace url_shortener_api.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; } 

		public string Name { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public Role Role { get; set; } = new Role();

		[JsonIgnore]
		public ICollection<URL> URLs { get; set; } = new List<URL>();
	}

	public class UserDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;
	}
}
