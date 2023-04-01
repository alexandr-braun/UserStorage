using UserStorage.Domain.User.ValueObjects;
using UserStorage.Domain.User.ValueObjects.Identifiers;

namespace UserStorage.Domain.User;

public class User
{
    public User(UserId userId, string userName, UserContacts userContacts)
    {
        Id = userId;
        Name = userName;
        Contacts = userContacts;
    }

    public UserId Id { get;}
    public string Name { get;}

    public UserContacts Contacts { get; }
}
