namespace UserStorage.Presentation.Consumers.User.Contracts;

public class UserContract
{
    public Guid Id { get;}

    public string Name { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string Address { get; set; }
}