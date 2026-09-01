using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static E_Commerce.Infrastructure.Identity.Services.TokenService;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class TokenService(IOptions<JWTSettings> jwtOptions) : ITokenService
    {
        private readonly JWTSettings _Settings = jwtOptions.Value;
        public string CreateToken(
          string userId,
          string email,
          string UserName,
          IEnumerable<string> roles)
        {
            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, userId),
        new(ClaimTypes.Email, email),
        new(ClaimTypes.Name, UserName)
    };

            claims.AddRange(
                roles.Select(r => new Claim(ClaimTypes.Role, r))
            );

            if (string.IsNullOrEmpty(_Settings.SecretKey))
            {
                throw new InvalidOperationException("JWT SecretKey Is Missing");
            }

            if (_Settings.SecretKey.Length < 32)
            {
                throw new InvalidOperationException("JWT SecretKey Is Too Short");
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_Settings.SecretKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _Settings.Issuer,
                audience: _Settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _Settings.ExpirationMinutes
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public class JWTSettings
        {
            public string SecretKey { get; init; } = default!;
            public string Issuer { get; init; } = default!;
            public string Audience { get; init; } = default!;
            public int ExpirationMinutes { get; init; } = 60;
        }
    }
}
