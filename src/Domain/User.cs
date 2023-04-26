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
        Id = Guid.NewGuid();
        profiles = new List<AuthorProfile>();
    }

    private readonly List<AuthorProfile> profiles;

    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public UserSettings Settings { get; private set; }

    public IEnumerable<AuthorProfile> Profiles => profiles.AsReadOnly();
}

public class UserSettings
{
    public UserSettings()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; private set; }
    public UserTier Tier { get; private set; }
}

public record UserTier(int Id, string TierName);
