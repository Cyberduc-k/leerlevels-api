using Model;

namespace Service.Interfaces;

public interface IProgressService
{
    public Task<ICollection<TargetProgress>> GetAllTargetProgress(string userId);

    public Task<SetProgress> GetSetProgress(string setId);
    public Task<TargetProgress> GetTargetProgress(string targetId, string userId);
    public Task<McqProgress> GetMcqProgress(string mcqId, string userId);

    public Task<TargetProgress> BeginTarget(string targetId, string userId);
    public Task<McqProgress> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind, string userId);
}
