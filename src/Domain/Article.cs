namespace Domain;

/*
 * Exercise class
 *
 * Currently, this class corresponds to what beginner .NET developers create. Let's make it worthy of
 * software professionals' standards.
 *
 * 1. Protect the class' invariants.
 * 2. 
 */
public class Article
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Content { get; set; }
    
    /// <summary>
    /// The image that is commonly displayed at the top of an article.
    /// </summary>
    public byte[]? PictureLead { get; set; }
}

public record CategoryTag(string TagName);
