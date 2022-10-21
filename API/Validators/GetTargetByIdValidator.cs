using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model;
using Repository.Interfaces;

namespace API.Validators;
public class GetTargetByIdValidator : AbstractValidator<Target>
{
    private readonly ITargetRepository targetRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTargetByIdValidator"/> class.
    /// </summary>
    /// <param name="targetRepo"></param>
    public GetTargetByIdValidator(ITargetRepository targetRepo)
    {
        this.targetRepo = targetRepo;
        RuleFor(x => x.Id).MustAsync(IsTargetExistsAsync);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTargetByIdValidator"/> class.
    /// </summary>
    public GetTargetByIdValidator()
    {
        RuleFor(x => x.Id).MustAsync(IsTargetExistsAsync);
    }

    private async Task<bool> IsTargetExistsAsync(string targetId, CancellationToken cancellation)
    {
        bool isExists = await this.targetRepo.AnyAsync(x => x.Id == targetId);

        return isExists;
    }
}
