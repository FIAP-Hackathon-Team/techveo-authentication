using System.ComponentModel.DataAnnotations;

namespace TechVeo.Authentication.Contracts.Authentication;

public record RegisterRequest(
    [Required] string FullName,
    [Required] string Username,
    string? Email,
    [Required] string Password);
