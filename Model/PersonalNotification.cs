namespace Model;

public class PersonalNotification : Notification
{
    public string UserId { get; set; }

    public PersonalNotification()
    {
    }

    public PersonalNotification(string userId, string title, string message) : base(title, message)
    {
        UserId = userId;
    }
}
