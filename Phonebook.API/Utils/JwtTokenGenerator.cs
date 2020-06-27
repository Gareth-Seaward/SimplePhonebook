using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Phonebook.API.Models;

namespace Phonebook.API.Utils
{
  public class JwtTokenGenerator : IJwtTokenGenerator
  {
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
      _config = config;
    }
    public string GetJwt(User user)
    {
      var claims = CreateClaims(user);
      var key = CreateSecurityKey();
      var creds = CreateSigningCredentials(key);
      var tokenDescriptor = CreateDescriptor(claims, creds);

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    private static SecurityTokenDescriptor CreateDescriptor(Claim[] claims, SigningCredentials creds)
    {
      return new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };
    }

    private static SigningCredentials CreateSigningCredentials(SymmetricSecurityKey key)
    {
      return new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    }

    private SymmetricSecurityKey CreateSecurityKey()
    {
      return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
    }

    private static Claim[] CreateClaims(User user)
    {
      return new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
      };
    }
  }
}