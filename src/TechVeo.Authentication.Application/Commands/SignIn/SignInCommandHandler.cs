using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TechVeo.Authentication.Application.Dto;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.Repositories;

namespace TechVeo.Authentication.Application.Commands.SignIn;

public class SignInCommandHandler(
        IUserRepository repo,
        IConfiguration configuration)
            : IRequestHandler<SignInCommand, SignInResultDto>
{
    private const string Issuer = "techveo-jwts";
    private const string Audience = "techveo";

    private static readonly TimeSpan _tokenExpiration = TimeSpan.FromHours(1);

    public async Task<SignInResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await repo.GetByUsernameOrEmailAsync(request.Username);
        if (user == null)
        {
            throw new Shared.Application.Exceptions.ApplicationException(Resources.Exceptions.Auth_InvalidUseOrPassword);
        }

        var validation = new PasswordHasher<User>();
        var isValid = validation.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (isValid == PasswordVerificationResult.Failed)
        {
            throw new Shared.Application.Exceptions.ApplicationException(Resources.Exceptions.Auth_InvalidUseOrPassword);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, user.Role)
            },
            expires: DateTime.UtcNow.Add(_tokenExpiration),
            audience: Audience,
            issuer: Issuer,
            signingCredentials: creds);

        return new(
            new JwtSecurityTokenHandler().WriteToken(token),
            null!,
            (int)_tokenExpiration.TotalSeconds,
            new(
                user.Id,
                user.Name.FullName,
                user.Username,
                user.Email?.Address,
                user.Role)
            );
    }
}
