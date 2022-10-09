﻿namespace Model;

public class Forum
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public User From { get; set; }

    public Forum()
    {

    }

    public Forum(string id, string title, string description, User from)
    {
        Id = id;
        Title = title;
        Description = description;
        From = from;
    }
}
