using Microsoft.EntityFrameworkCore;
using url_shortener_api.Context;

namespace url_shortener_api.Service
{
	public class UrlShorteningService
	{
		public const int NumberOfCharsInShortLink = 7;
		private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		private readonly Random random = new Random();
		private readonly URLContext context;

		public UrlShorteningService(URLContext context)
		{
			this.context = context;
		}

		public async Task<string> GenerateUniqueCode()
		{
			var codeChars = new char[NumberOfCharsInShortLink];

			while (true)
			{
				for(var i = 0; i < NumberOfCharsInShortLink; i++)
				{
					int randomIndex = random.Next(Alphabet.Length - 1);

					codeChars[i] = Alphabet[randomIndex];
				}

				var code = new string(codeChars);

				if(!await context.URLs.AnyAsync(x => x.Code == code))
				{
					return code;
				}
			}
		}
	}
}
