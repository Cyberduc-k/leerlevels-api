using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class BookmarkDTO
{
    [OpenApiProperty(Description = "The id of a bookmark", Nullable = false)]
    public string ItemId { get; set; }

    [OpenApiProperty(Description = "The type of a bookmark (target or question)", Nullable = false)]
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
