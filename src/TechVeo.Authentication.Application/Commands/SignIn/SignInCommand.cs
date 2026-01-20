using MediatR;
using TechVeo.Authentication.Application.Dto;

namespace TechVeo.Authentication.Application.Commands.SignIn;

public record SignInCommand(string Username, string Password) : IRequest<SignInResultDto>;
