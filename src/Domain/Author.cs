namespace Domain;

public class Author
{
    public Author(string penName)
    {
        Id = Guid.NewGuid();
        PenName = penName;
    }
    
    public Guid Id { get; }
    
    /**
     * An artistic name the author has picked - or the author's own name.
     */
    public string PenName { get; set; }
}