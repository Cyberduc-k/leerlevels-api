namespace Model;

public class ForumReply
{
    public string Id { get; set; }
    public User From { get; set; }
    public string Text { get; set; }

    public ForumReply()
    {
    }

    public ForumReply(string id, User from, string text)
    {
        Id = id;
        From = from;
        Text = text;
    }
}
