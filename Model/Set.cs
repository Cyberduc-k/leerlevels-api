namespace Model;

public class Set
{
    public string Id { get; set; }
    public virtual ICollection<Target> Targets { get; set; }

    public Set()
    {

    }
    public Set(string id, ICollection<Target> targets)
    {
        Id = id;
        Targets = targets;
    }
}
