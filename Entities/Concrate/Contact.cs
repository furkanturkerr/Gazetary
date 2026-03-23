namespace Entities.Concrate;

public class Contact
{
    public int ContactId { get; set; }
    public string NameSurname { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}