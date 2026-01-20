using System.ComponentModel.DataAnnotations;

namespace TechVeo.Authentication.Contracts.Authentication;

public record SignInRequest(
    [Required] string Username,
    [Required] string Password);
