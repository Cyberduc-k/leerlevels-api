namespace Model;

public class AnswerOption
{
    public string Id { get; set; }
    public int Index { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public AnswerOption()
    {
    }

    public AnswerOption(string id, int index, string text, bool isCorrect)
    {
        Id = id;
        Index = index;
        Text = text;
        IsCorrect = isCorrect;
    }
}
