using UserStorage.Domain.User.ValueObjects;
using UserStorage.Domain.User.ValueObjects.Identifiers;

namespace UserStorage.Domain.User;

public class User
{
    public User(UserId userId, UserName userName, UserContacts userContacts)
    {
        Id = userId;
        Name = userName;
        Contacts = userContacts;
    }

    public UserId Id { get;}
    public UserName Name { get;}

    public UserContacts Contacts { get; }
}
