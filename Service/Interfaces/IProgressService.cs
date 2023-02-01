using Model;

namespace Service.Interfaces;

public interface IProgressService
{
    /**
     * <summary>
     * Get all target progress for the given user.
     * </summary>
     */
    public Task<ICollection<TargetProgress>> GetAllTargetProgress(string userId);

    /**
     * <summary>
     * Get a summary of all the progress for the given user.
     * </summary>
     */
    public Task<UserProgress> GetUserProgress(string userId);

    /**
     * <summary>
     * Get the user's progress for the given set.
     * </summary>
     */
    public Task<SetProgress> GetSetProgress(string setId, string userId);

    /**
     * <summary>
     * Get the user's progress for the given target.
     * </summary>
     */
    public Task<TargetProgress> GetTargetProgress(string targetId, string userId);
    
    /**
     * <summary>
     * Get the user's progress for the given mcq.
     * </summary>
     */
    public Task<McqProgress> GetMcqProgress(string mcqId, string userId);

    /**
     * <summary>
     * Begin a new <see cref="TargetProgress"/> for the given target and user.
     * </summary>
     * <remarks>Does nothing if the user already has progress for this target.</remarks>
     * <param name="reset">reset the progress</param>
     */
    public Task<(TargetProgress, bool)> BeginTarget(string targetId, string userId, bool reset);

    /**
     * <summary>
     * Answer the given mcq using the <paramref name="answerId"/> and <paramref name="answerKind"/>.
     * </summary>
     * <remarks>Does nothing if the question is already answered correctly or if the same answer was given previously.</remarks>
     */
    public Task<(McqProgress, bool)> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind, string userId);
}
