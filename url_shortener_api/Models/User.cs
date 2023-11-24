using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace url_shortener_api.Models
{
	public class User
	{
		[Key]
		public int id { get; set; }

		public string name { get; set; }

		public string password { get; set; }

		public Role role { get; set; }

		[JsonIgnore]
		public ICollection<URL> urls { get; set; }
	}
}
