using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model;
using Repository.Interfaces;

namespace API.Validators;
public class GetMcqByIdValidator : AbstractValidator<Mcq>
{
    private readonly IMcqRepository mcqRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMcqByIdValidator"/> class.
    /// </summary>
    /// <param name="mcqRepo"></param>
    public GetMcqByIdValidator(IMcqRepository mcqRepo)
    {
        this.mcqRepo = mcqRepo;
        RuleFor(x => x.Id).MustAsync(IsMcqExistsAsync);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMcqByIdValidator"/> class.
    /// </summary>
    public GetMcqByIdValidator()
    {
        RuleFor(x => x.Id).MustAsync(IsMcqExistsAsync);
    }

    private async Task<bool> IsMcqExistsAsync(string mcqId, CancellationToken cancellation)
    {
        bool isExists = await this.mcqRepo.AnyAsync(x => x.Id == mcqId);

        return isExists;
    }
}
