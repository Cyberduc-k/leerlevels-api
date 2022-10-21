using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model;
using Repository.Interfaces;

namespace API.Validators;
public class GetSetByIdValidator : AbstractValidator<Set>
{
    private readonly ISetRepository setRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSetByIdValidator"/> class.
    /// </summary>
    /// <param name="setRepo"></param>
    public GetSetByIdValidator(ISetRepository setRepo)
    {
        this.setRepo = setRepo;
        RuleFor(x => x.Id).MustAsync(IsSetExistsAsync);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSetByIdValidator"/> class.
    /// </summary>
    public GetSetByIdValidator()
    {
        RuleFor(x => x.Id).MustAsync(IsSetExistsAsync);
    }

    private async Task<bool> IsSetExistsAsync(string setId, CancellationToken cancellation)
    {
        bool isExists = await this.setRepo.AnyAsync(x => x.Id == setId);

        return isExists;
    }
}
