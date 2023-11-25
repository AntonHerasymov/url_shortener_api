using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using url_shortener_api.Config;
using url_shortener_api.Models;

namespace url_shortener_api.utils
{
	public class Token
	{
		public static string CreateToken(User user)
		{
			ClaimsIdentity identity = GetIdentity(user);
			var now = DateTime.UtcNow;

			var jwt = new JwtSecurityToken(
			   issuer: AuthOptions.ISSUER,
			audience: AuthOptions.AUDIENCE,
			notBefore: now,
			claims: identity.Claims,
			expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
			   signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
			return encodedJwt;
		}

		public static ClaimsIdentity GetIdentity(User user)
		{
			var claims = new List<Claim>
				{
					new Claim("Id", Convert.ToString(user.Id)),
					new Claim("Name", Convert.ToString(user.Login)),
					new Claim("Role", Convert.ToString(user.Role.Id))
				};
			ClaimsIdentity claimsIdentity =
			new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			return claimsIdentity;
		}
	}
}
