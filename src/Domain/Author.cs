namespace Domain;

public class Author
{
    public Guid Id { get; set; }
    
    /**
     * An artistic name the author has picked - or the author's own name.
     */
    public string PenName { get; set; }
}