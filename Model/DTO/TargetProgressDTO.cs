namespace Model.DTO;

public class TargetProgressDTO
{
    public string TargetId { get; set; }

    public TargetProgressDTO()
    {
    }

    public TargetProgressDTO(string targetId)
    {
        TargetId = targetId;
    }
}
