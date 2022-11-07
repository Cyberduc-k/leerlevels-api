using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class BookmarkDTOExample : OpenApiExample<BookmarkDTO>
{
    public override IOpenApiExample<BookmarkDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("BookmarkDTO_Target", new BookmarkDTO() {
            ItemId = Guid.NewGuid().ToString(),
            Type = Model.Bookmark.BookmarkType.Target,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("BookmarkDTO_Mcq", new BookmarkDTO() {
            ItemId = Guid.NewGuid().ToString(),
            Type = Model.Bookmark.BookmarkType.Mcq,
        }, namingStrategy));

        return this;
    }
}
