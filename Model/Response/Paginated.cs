namespace Model.Response;

public class Paginated<T>
{
    public T[] Items { get; set; }
    public int Page { get; set; }

    public Paginated()
    {
    }

    public Paginated(T[] items, int page)
    {
        Items = items;
        Page = page;
    }
}
