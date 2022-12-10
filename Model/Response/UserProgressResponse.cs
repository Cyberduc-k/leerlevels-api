using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.Response;

public class UserProgressResponse
{
    [OpenApiProperty(Description = "The % of targets completed")]
    public int TargetsCompleted { get; set; }

    [OpenApiProperty(Description = "The % of sets completed")]
    public int SetsCompleted { get; set; }

    [OpenApiProperty(Description = "The average score of all targets")]
    public int AverageScore { get; set; }

    public UserProgressResponse()
    {
    }

    public UserProgressResponse(int targetsCompleted, int setsCompleted, int averageScore)
    {
        TargetsCompleted = targetsCompleted;
        SetsCompleted = setsCompleted;
        AverageScore = averageScore;
    }
}
