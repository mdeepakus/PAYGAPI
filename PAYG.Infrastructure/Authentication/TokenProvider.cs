using Microsoft.Extensions.Options;
using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Authentication
{
    public class TokenProvider : ITokenProvider
    {
        private TokenProviderOptions _options;

        public TokenProvider(IOptions<TokenProviderOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var handler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String)
            };

            var genericIdentity = new GenericIdentity(user.UserId.ToString(), "TokenAuth");
            ClaimsIdentity identity = new ClaimsIdentity(genericIdentity, claims);
 
            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = _options.SigningCredentials,
                Subject = identity,
                Expires = now.Add(_options.Expiration),
                NotBefore = now,
                IssuedAt = now
            });

            var encodedToken = handler.WriteToken(securityToken);

            return encodedToken;
        }
    }
}
