using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class ProgressService : IProgressService
{
    private readonly ITokenService _tokenService;
    private readonly ITargetProgressRepository _targetProgressRepository;
    private readonly IMcqProgressRepository _mcqProgressRepository;

    public ProgressService(ITokenService tokenService, ITargetProgressRepository targetProgressRepository, IMcqProgressRepository mcqProgressRepository)
    {
        _tokenService = tokenService;
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
        McqProgress mcqProgress = await _mcqProgressRepository.GetByIdAsync((_tokenService.User.Id, mcq.Id)) ?? throw new NotFoundException("mcq progress");

        mcqProgress.Answer = answer;
        await _mcqProgressRepository.SaveChanges();
    }
}
