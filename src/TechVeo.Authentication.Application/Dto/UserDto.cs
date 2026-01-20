using System;

namespace TechVeo.Authentication.Application.Dto;

public record UserDto(Guid Id, string Name, string Username, string? Email, string Role);
