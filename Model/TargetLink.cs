namespace Model;

public class TargetLink
{
    public string FromId { get; set; }
    public string ToId { get; set; }

    public Target From { get; set; }
    public Target To { get; set; }

    public TargetLink()
    {
    }

    public TargetLink(Target from, Target to)
    {
        FromId = from.Id;
        ToId = to.Id;
        From = from;
        To = to;
    }
}
