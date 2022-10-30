using Model;

namespace Service.Interfaces;

public interface IProgressService
{
    public Task<ICollection<TargetProgress>> GetAllTargetProgress();
    public Task<TargetProgress> BeginTarget(string targetId);
    public Task<McqProgress> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind);
}
