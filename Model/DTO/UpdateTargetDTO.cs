using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateTargetDTO
{
    [OpenApiProperty(Default = null, Description = "The label or name of this target", Nullable = true)]
    public string? Label { get; set; }

    [OpenApiProperty(Default = null, Description = "The description of this target", Nullable = true)]
    public string? Description { get; set; }

    [OpenApiProperty(Default = null, Description = "The explanation of this target", Nullable = true)]
    public string? TargetExplanation { get; set; }

    [OpenApiProperty(Default = null, Description = "The id of a youtube video for this target", Nullable = true)]
    public string? YoutubeId { get; set; }

    [OpenApiProperty(Default = null, Description = "The url to an image for this target", Nullable = true)]
    public string? ImageUrl { get; set; }
}
