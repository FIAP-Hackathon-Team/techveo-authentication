using System;
using MediatR;
using TechVeo.Authentication.Application.Dto;

namespace TechVeo.Authentication.Application.Queries.GetUser;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
