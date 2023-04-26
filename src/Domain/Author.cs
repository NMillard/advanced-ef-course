namespace Domain;

public class Author
{
    public Author(string penName)
    {
        Id = Guid.NewGuid();
        PenName = penName;
        articles = new List<Article>();
    }
    
    private List<Article> articles;

    public Guid Id { get; private set; }

    /**
     * An artistic name the author has picked - or the author's own name.
     */
    public string PenName { get; set; }

    public IEnumerable<Article> Articles => articles.AsReadOnly();
}