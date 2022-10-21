﻿namespace Model;

public class Set
{
    public string Id { get; set; }
    public virtual ICollection<Target> Targets { get; set; }

    //public virtual ICollection<User> Users { get; set; }

    public Set()
    {

    }
    public Set(string id, ICollection<Target> targets/*, ICollection<User> users*/)
    {
        Id = id;
        Targets = targets;
        //Users = users;
    }
}
