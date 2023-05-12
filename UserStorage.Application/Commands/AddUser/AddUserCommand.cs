using MediatR;

namespace UserStorage.Application.Commands.AddUser;

public class AddUserCommand : IRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string Address { get; set; }
}