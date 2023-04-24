namespace Domain;

/*
 * Set up a configuration for this type.
 * 
 * Add constructors as later exercise.
 */

public class User
{
    public User()
    {
        profiles = new List<Author>();
    }

    private readonly List<Author> profiles;

    public Guid Id { get; set; }
    public string Username { get; set; }
    public UserSettings UserSettings { get; set; }

    public IEnumerable<Author> Profiles => profiles.AsReadOnly();
}

public class UserSettings
{
    public Guid Id { get; set; }
    public UserTier Tier { get; set; }
}

public record UserTier(int Id, string TierName);
