using Model;

namespace Service.Interfaces;

public interface IProgressService
{
    public Task<ICollection<TargetProgress>> GetAllTargetProgress(string userId);

    public Task<UserProgress> GetUserProgress(string userId);
    public Task<SetProgress> GetSetProgress(string setId, string userId);
    public Task<TargetProgress> GetTargetProgress(string targetId, string userId);
    public Task<McqProgress> GetMcqProgress(string mcqId, string userId);

    public Task<(TargetProgress, bool)> BeginTarget(string targetId, string userId, bool reset);
    public Task<(McqProgress, bool)> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind, string userId);
}
