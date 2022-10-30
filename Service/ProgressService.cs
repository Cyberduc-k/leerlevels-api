﻿using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class ProgressService : IProgressService
{
    private readonly ITokenService _tokenService;
    private readonly INotificationService _notificationService;
    private readonly ISetService _setService;
    private readonly ITargetService _targetService;
    private readonly IMcqService _mcqService;
    private readonly ITargetProgressRepository _targetProgressRepository;
    private readonly IMcqProgressRepository _mcqProgressRepository;
    private readonly IAnswerOptionRepository _answerOptionRepository;

    public ProgressService(
        ITokenService tokenService,
        INotificationService notificationService,
        ISetService setService,
        ITargetService targetService,
        IMcqService mcqService,
        ITargetProgressRepository targetProgressRepository,
        IMcqProgressRepository mcqProgressRepository,
        IAnswerOptionRepository answerOptionRepository)
    {
        _tokenService = tokenService;
        _notificationService = notificationService;
        _setService = setService;
        _targetService = targetService;
        _mcqService = mcqService;
        _targetProgressRepository = targetProgressRepository;
        _mcqProgressRepository = mcqProgressRepository;
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task<ICollection<TargetProgress>> GetAllTargetProgress()
    {
        return await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answer)
            .GetAllWhereAsync(t => t.User.Id == _tokenService.User.Id)
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
            .ThenInclude(m => m.Answer)
            .GetAllWhereAsync(t => set.Targets.Contains(t.Target))
            .ToArrayAsync();

        return new SetProgress(set, targets);
    }

    public async Task<TargetProgress> GetTargetProgress(string targetId)
    {
        return await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Mcq)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answer)
            .GetByAsync(t => t.User.Id == _tokenService.User.Id && t.Target.Id == targetId)
            ?? throw new NotFoundException("target progress");
    }

    public async Task<McqProgress> GetMcqProgress(string mcqId)
    {
        return await _mcqProgressRepository
            .Include(m => m.Mcq)
            .Include(m => m.Answer)
            .GetByAsync(m => m.User.Id == _tokenService.User.Id && m.Mcq.Id == mcqId)
            ?? throw new NotFoundException("mcq progress");
    }

    public async Task<TargetProgress> BeginTarget(string targetId)
    {
        Target target = await _targetService.GetTargetWithMcqByIdAsync(targetId);
        TargetProgress? existing = await _targetProgressRepository
            .Include(t => t.Target)
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.Answer)
            .GetByAsync(t => t.User.Id == _tokenService.User.Id && t.Target.Id == targetId);

        if (existing is not null)
            return existing;

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
        return targetProgress;
    }

    public async Task<McqProgress> AnswerQuestion(string mcqId, string answerId, AnswerKind answerKind)
    {
        Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);
        AnswerOption answer = await _answerOptionRepository.GetByIdAsync(answerId) ?? throw new NotFoundException("answer option");
        TargetProgress targetProgress = await _targetProgressRepository
            .Include(t => t.Mcqs)
            .GetByAsync(t => t.User.Id == _tokenService.User.Id && t.Target.Id == mcq.Target.Id)
            ?? throw new NotFoundException("target progress");
        McqProgress mcqProgress = targetProgress.Mcqs.FirstOrDefault(m => m.Mcq.Id == mcq.Id) ?? throw new NotFoundException("mcq progress");

        mcqProgress.Answer = answer;
        mcqProgress.AnswerKind = answerKind;
        await _mcqProgressRepository.SaveChanges();

        if (targetProgress.IsCompleted) {
            PersonalNotification notification = new(_tokenService.User.Id, "Target completed", $"You have completed target {targetProgress.Target.Label}");

            await _notificationService.SendNotificationAsync(notification);
        }

        return mcqProgress;
    }
}
