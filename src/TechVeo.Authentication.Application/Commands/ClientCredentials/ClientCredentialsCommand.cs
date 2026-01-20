using MediatR;
using TechVeo.Authentication.Application.Dto;

namespace TechVeo.Authentication.Application.Commands.ClientCredentials;

public record ClientCredentialsCommand(
    string ClientId,
    string ClientSecret,
    string? Scope = null
) : IRequest<TokenResultDto>;
