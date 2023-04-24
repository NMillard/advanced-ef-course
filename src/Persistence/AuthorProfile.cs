using Domain;

namespace Persistence;

public class AuthorProfile
{
    public User User { get; set; }
    public Author Author { get; set; }
    public bool IsAdministrator { get; set; }
}