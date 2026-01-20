using System;
using System.Threading.Tasks;
using TechVeo.Authentication.Domain.Entities;

namespace TechVeo.Authentication.Domain.Repositories;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user);

    Task<User?> GetByUsernameOrEmailAsync(string username);
}
