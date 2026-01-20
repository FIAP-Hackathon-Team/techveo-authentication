using System;
using TechVeo.Shared.Domain.Exceptions;
using TechVeo.Shared.Domain.Validations;
using TechVeo.Shared.Domain.ValueObjects;

namespace TechVeo.Authentication.Domain.ValueObjects;

public class Email : ValueObject, IEquatable<Email>
{
    public Email(string address)
    {
        Address = address;
        Validate();
    }

    public string Address { get; set; }

    public bool Equals(Email? other)
    {
        return other is not null &&
               Address == other.Address;
    }

    public static implicit operator Email(string address) => new(address);

    public static implicit operator string(Email email) => email.Address;
    private void Validate()
    {
        if (!ValidateEmail.IsValidEmail(Address))
        {
            throw new DomainException(Resources.Exceptions.Customer_ThrowEmailInvalid);
        }
    }
}
