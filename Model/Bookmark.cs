namespace Model;

public class Bookmark
{
    public enum BookmarkType
    {
        Target,
        Mcq,
    }

    public string UserId { get; set; }
    public string ItemId { get; set; }
    public BookmarkType Type { get; set; }

    public Bookmark()
    {
    }

    public Bookmark(string itemId, BookmarkType type)
    {
        ItemId = itemId;
        Type = type;
    }
}
