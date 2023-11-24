using Microsoft.EntityFrameworkCore;
using url_shortener_api.Models;
using url_shortener_api.Service;

namespace url_shortener_api.Context
{
	public class URLContext : DbContext
	{
		public URLContext()
		{
			Database.EnsureCreated();
			EnsureRolesCreated();
		}

		public URLContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
			EnsureRolesCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured)
			{
				options.UseSqlServer("A FALLBACK CONNECTION STRING");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<URL>(builder =>
			{
				builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);

				builder.HasIndex(s => s.Code).IsUnique();
			});
		}

		private void EnsureRolesCreated()
		{
			if (!Roles.Any())
			{
				Roles.Add(new Role { Name = "admin" });
				Roles.Add(new Role { Name = "user" });
				SaveChanges();
			}
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<URL> URLs { get; set; }
	}
}
