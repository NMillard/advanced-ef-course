namespace Domain;


// TODO: Add Tags list and update entity configuration
public class Article
{
    private readonly List<CategoryTag> tags;
    
    public Article()
    {
        tags = new List<CategoryTag>();
    }

    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Content { get; set; }
    
    /// <summary>
    /// The image that is commonly displayed at the top of an article.
    /// </summary>
    public byte[]? PictureLead { get; set; }

    public IEnumerable<CategoryTag> Tags => tags.AsReadOnly();
}

public record CategoryTag(string TagName);
