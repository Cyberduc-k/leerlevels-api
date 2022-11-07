namespace Model.DTO;

public class BookmarkDTO
{
    public string ItemId { get; set; }
    public Bookmark.BookmarkType Type { get; set; }

    public BookmarkDTO()
    {
    }

    public BookmarkDTO(string itemId, Bookmark.BookmarkType type)
    {
        ItemId = itemId;
        Type = type;
    }
}
