using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class ProgressService : IProgressService
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;
    private readonly ISetService _setService;
    private readonly ITargetService _targetService;
    private readonly IMcqService _mcqService;
    private readonly ITargetProgressRepository _targetProgressRepository;
    private readonly IMcqProgressRepository _mcqProgressRepository;
    private readonly IAnswerOptionRepository _answerOptionRepository;

    public ProgressService(
        IUserService userService,
        INotificationService notificationService,
        ISetService setService,
        ITargetService targetService,
        IMcqService mcqService,
        ITargetProgressRepository targetProgressRepository,
        IMcqProgressRepository mcqProgressRepository,
        IAnswerOptionRepository answerOptionRepository)
    {
        _userService = userService;
        _notificationService = notificationService;
        _setService = setService;
        _targetService = targetService;
        _mcqService = mcqService;
        _targetProgressRepository = targetProgressRepository;
        _mcqProgressRepository = mcqProgressRepository;
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task<ICollection<TargetProgress>> GetAllTargetProgress(string userId)
    {
        return await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answers)
            .ThenInclude(a => a.Answer)
            .Where(t => t.User.Id == userId)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<SetProgress> GetSetProgress(string setId)
    {
        Set set = await _setService.GetSetByIdAsync(setId);
        TargetProgress[] targets = await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answers)
            .ThenInclude(a => a.Answer)
            .Where(t => set.Targets.Contains(t.Target))
            .GetAllAsync()
            .ToArrayAsync();

        return new SetProgress(set, targets);
    }

    public async Task<TargetProgress> GetTargetProgress(string targetId, string userId)
    {
        return await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answers)
            .ThenInclude(a => a.Answer)
            .GetByAsync(t => t.User.Id == userId && t.Target.Id == targetId)
            ?? throw new NotFoundException("target progress");
    }

    public async Task<McqProgress> GetMcqProgress(string mcqId, string userId)
    {
        return await _mcqProgressRepository
            .Include(m => m.Mcq)
            .Include(m => m.Answers)
            .ThenInclude(a => a.Answer)
            .GetByAsync(m => m.User.Id == userId && m.Mcq.Id == mcqId)
            ?? throw new NotFoundException("mcq progress");
    }

    public async Task<TargetProgress> BeginTarget(string targetId, string userId, bool reset)
    {
        User user = await _userService.GetUserById(userId);
        Target target = await _targetService.GetTargetByIdAsync(targetId);
        TargetProgress? existing = await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answers)
            .ThenInclude(a => a.Answer)
            .GetByAsync(t => t.User.Id == userId && t.Target.Id == targetId);

        if (existing is not null) {
            if (reset) {
                foreach (McqProgress mcq in existing.Mcqs) {
                    _mcqProgressRepository.Remove(mcq);
                }

                existing.Mcqs = target.Mcqs.Select(m => new McqProgress() {
                    User = user,
                    Mcq = m
                }).ToList();

                await _targetProgressRepository.SaveChanges();
            }

            return existing;
        }

        TargetProgress targetProgress = new() {
            User = user,
            Target = target,
            Mcqs = target.Mcqs.Select(m => new McqProgress() {
                User = user,
                Mcq = m
            }).ToArray(),
        };

        await _targetProgressRepository.InsertAsync(targetProgress);
        await _targetProgressRepository.SaveChanges();
        return targetProgress;
    }

    public async Task<McqProgress> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind, string userId)
    {
        Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);
        AnswerOption answer = await _answerOptionRepository.GetByIdAsync(answerId) ?? throw new NotFoundException("answer option");
        TargetProgress targetProgress = await _targetProgressRepository
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answers)
            .GetByAsync(t => t.User.Id == userId && t.Target.Id == mcq.Target.Id)
            ?? throw new NotFoundException("target progress");
        McqProgress mcqProgress = targetProgress.Mcqs.FirstOrDefault(m => m.Mcq.Id == mcq.Id) ?? throw new NotFoundException("mcq progress");

        mcqProgress.AddAnswer(answer, answerKind);
        await _mcqProgressRepository.SaveChanges();

        if (targetProgress.IsCompleted()) {
            PersonalNotification notification = new(userId, "Target completed", $"You have completed target {targetProgress.Target.Label}");

            await _notificationService.SendNotificationAsync(notification);
        }

        return mcqProgress;
    }
}
