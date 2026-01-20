using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Infra.Persistence.Contexts;

namespace TechVeo.Authentication.Infra.Persistence.Repositories
{
    internal class UserRepository(AuthContext dbContext) : IUserRepository
    {
        private readonly AuthContext _dbContext = dbContext;

        public async Task<Guid> AddAsync(User user)
        {
            var entry = await _dbContext.AddAsync(user);

            return entry.Entity.Id;
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string username)
        {
            return await _dbContext
                .Users
                .FirstOrDefaultAsync(
                    u => u.Username == username || (u.Email != null && u.Email.Address! == username));
        }
    }
}
