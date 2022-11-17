namespace Model;

public class Set
{
    public string Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Target> Targets { get; set; }

    public virtual ICollection<User> Users { get; set; }

    public Set()
    {
    }

    public Set(string id, string name, ICollection<Target> targets)
    {
        Id = id;
        Name = name;
        Targets = targets;
    }
}
