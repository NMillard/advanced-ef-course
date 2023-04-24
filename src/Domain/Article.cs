namespace Domain;

public class Article
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    /// <summary>
    /// The image that is commonly displayed at the top of an article.
    /// </summary>
    public byte[] PictureLead { get; set; }
    
    
}

public record CategoryTag(long Id, string TagName);
