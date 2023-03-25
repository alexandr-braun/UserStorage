namespace UserStorage.Domain.User.ValueObjects;

public record struct UserContacts(string PhoneNumber, string EmailAddress, string Address);