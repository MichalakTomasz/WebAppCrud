using Domain.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAppCrud.Mediator
{
	public class GenerateTokenRequestHandlerAsync : IRequestHandler<GenerateTokenRequest, (string token, DateTime expiration)>
	{
		private readonly IConfiguration _configuration;

		public GenerateTokenRequestHandlerAsync(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public Task<(string token, DateTime expiration)> Handle(GenerateTokenRequest request, CancellationToken cancellationToken)
		{
			List<Claim> claims = new()
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			if (request.InputRoles?.Roles != default)
			{
				request.InputRoles?.Roles?.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
			}
			else
			{
				claims.Add(new Claim(ClaimTypes.Role, CommonConsts.Guest));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[CommonConsts.JwtKey]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken(
				claims: claims,
				expires: expiration,
				issuer: _configuration[CommonConsts.Issuer],
				audience: _configuration[CommonConsts.Audience],
				signingCredentials: creds);

			return Task.FromResult((new JwtSecurityTokenHandler().WriteToken(token), expiration));
		}
	}
}
