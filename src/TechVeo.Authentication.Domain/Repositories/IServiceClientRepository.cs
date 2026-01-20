using System;
using System.Threading.Tasks;
using TechVeo.Authentication.Domain.Entities;

namespace TechVeo.Authentication.Domain.Repositories;

public interface IServiceClientRepository
{
    Task<ServiceClient?> GetByClientIdAsync(string clientId);
    Task UpdateLastUsedAsync(Guid id, DateTime lastUsedAt);
}
