using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TechVeo.Authentication.Application.Dto;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Domain.ValueObjects;

namespace TechVeo.Authentication.Application.Commands.Register;

public class RegisterCommandHandler(IUserRepository repo) : IRequestHandler<RegisterCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByUsernameOrEmailAsync(request.Username);
        if (existing != null)
        {
            throw new Shared.Application.Exceptions.ApplicationException("User already exists");
        }

        var name = new Name(request.FullName);
        Email? email = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            email = new Email(request.Email!);
        }

        var user = new User(name, request.Username, "user", email);

        var hasher = new PasswordHasher<User>();
        var hash = hasher.HashPassword(user, request.Password);

        user.SetPassword(hash);

        var id = await repo.AddAsync(user);

        return new UserDto(id, user.Name.FullName, user.Username, user.Email?.Address, user.Role);
    }
}
