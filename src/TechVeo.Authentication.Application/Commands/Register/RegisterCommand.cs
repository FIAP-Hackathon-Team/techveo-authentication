using MediatR;
using TechVeo.Authentication.Application.Dto;

namespace TechVeo.Authentication.Application.Commands.Register;

public record RegisterCommand(string FullName, string Username, string? Email, string Password) : IRequest<UserDto>;
