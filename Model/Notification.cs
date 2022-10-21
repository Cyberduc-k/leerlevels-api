namespace Model;

public class Notification
{
    public string Title { get; set; }
    public string Message { get; set; }

    public Notification()
    {
    }

    public Notification(string title, string message)
    {
        Title = title;
        Message = message;
    }
}
