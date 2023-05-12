using UserStorage.Application.Commands.AddUser;
using UserStorage.Presentation.Consumers.User.Contracts;

namespace UserStorage.Presentation.Consumers.User.Converters;

internal static class UserContractConverter
{
    internal static AddUserCommand ToAddUserCommand(UserContract userContract)
    {
        AddUserCommand result = new AddUserCommand
        {
            Id = userContract.Id,
            Name = userContract.Name,
            Address = userContract.Address,
            EmailAddress = userContract.EmailAddress
        };

        return result;
    }
}