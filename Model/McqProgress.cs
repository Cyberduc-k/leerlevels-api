namespace Model;

public class McqProgress
{
    public int Id { get; set; }
    public User User { get; set; }
    public Mcq Mcq { get; set; }
    public AnswerOption? Answer { get; set; }

    public McqProgress()
    {
    }

    public McqProgress(int id, User user, Mcq mcq, AnswerOption? answer)
    {
        Id = id;
        User = user;
        Mcq = mcq;
        Answer = answer;
    }
}
