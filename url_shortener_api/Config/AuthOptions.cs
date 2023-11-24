using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace url_shortener_api.Config
{
	public class AuthOptions
	{
			public const string ISSUER = "url_shortener_api";
			public const string AUDIENCE = "url_shortener_client";
			const string KEY = "9jFv%$vN7#qR2mUzX*pYsL@kG3hA6bE8"; 
			public const int LIFETIME = 10; 

			public static SymmetricSecurityKey GetSymmetricSecurityKey()
			{
				return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
			}
	}
}
