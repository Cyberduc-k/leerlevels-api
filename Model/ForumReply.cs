namespace Model;

public class ForumReply
{
    public string Id { get; set; }
    public User From { get; set; }

    public ForumReply()
    {

    }

    public ForumReply(string id, User from)
    {
        Id = id;
        From = from;
    }
}
