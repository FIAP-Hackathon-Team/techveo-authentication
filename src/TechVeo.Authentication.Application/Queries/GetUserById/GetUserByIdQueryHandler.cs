using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechVeo.Authentication.Application.Dto;
using TechVeo.Authentication.Domain.Repositories;

namespace TechVeo.Authentication.Application.Queries.GetUser;

public class GetUserByIdQueryHandler(IUserRepository repo) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(request.Id);
        if (user == null)
        {
            return null;
        }

        return new UserDto(user.Id, user.Name.FullName, user.Username, user.Email?.Address, user.Role);
    }
}
