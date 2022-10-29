using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class ProgressService : IProgressService
{
    private readonly ITokenService _tokenService;
    private readonly INotificationService _notificationService;
    private readonly ITargetProgressRepository _targetProgressRepository;
    private readonly IMcqProgressRepository _mcqProgressRepository;

    public ProgressService(
        ITokenService tokenService,
        INotificationService notificationService,
        ITargetProgressRepository targetProgressRepository,
        IMcqProgressRepository mcqProgressRepository)
    {
        _tokenService = tokenService;
        _notificationService = notificationService;
        _targetProgressRepository = targetProgressRepository;
        _mcqProgressRepository = mcqProgressRepository;
    }

    public async Task BeginTarget(Target target)
    {
        TargetProgress targetProgress = new() {
            User = _tokenService.User,
            Target = target,
            Mcqs = target.Mcqs.Select(m => new McqProgress() {
                User = _tokenService.User,
                Mcq = m
            }).ToArray(),
        };

        await _targetProgressRepository.InsertAsync(targetProgress);
        await _targetProgressRepository.SaveChanges();
    }

    public async Task AnswerQuestion(Mcq mcq, AnswerOption answer)
    {
        TargetProgress targetProgress = await _targetProgressRepository
            .Include(t => t.Mcqs)
            .GetByAsync(t => t.User.Id == _tokenService.User.Id && t.Target.Id == mcq.Target.Id)
            ?? throw new NotFoundException("target progress");
        McqProgress mcqProgress = targetProgress.Mcqs.FirstOrDefault(m => m.Mcq.Id == mcq.Id) ?? throw new NotFoundException("mcq progress");

        mcqProgress.Answer = answer;
        await _mcqProgressRepository.SaveChanges();

        if (targetProgress.IsComplete) {
            PersonalNotification notification = new(_tokenService.User.Id, "Target completed", $"You have completed target {targetProgress.Target.Label}");

            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
