namespace Model;
public class ForumReply
{
    public string Id { get; set; }
    public ICollection<User> From { get; set; }

    public ForumReply()
    {

    }

    public ForumReply(string id, ICollection<User> from)
    {
        Id = id;
        From = from;
    }
}
