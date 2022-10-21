using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Model;
using Repository.Interfaces;

namespace API.Validation;
public class GetGroupByIdValidator : AbstractValidator<Group>
{
    private readonly IGroupRepository groupRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupByIdValidator"/> class.
    /// </summary>
    /// <param name="groupRepo"></param>
    public GetGroupByIdValidator(IGroupRepository groupRepo)
    {
        this.groupRepo = groupRepo;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGroupByIdValidator"/> class.
    /// </summary>
    public GetGroupByIdValidator()
    {

    }

    private async Task<bool> IsGroupExistsAsync(Group group, CancellationToken cancellation)
    {
        var isExists = await this.groupRepo.FindByConditionAsync(x =>x.Id == group.Id);

        return isExists;
    }
}
