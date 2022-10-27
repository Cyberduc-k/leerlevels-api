using Model;

namespace Service.Interfaces;

public interface IProgressService
{
    public Task BeginTarget(Target target);
    public Task AnswerQuestion(Mcq mcq, AnswerOption answer);
}
