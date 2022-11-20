using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Model;
using Model.Response;
using Service;
using Service.Interfaces;

namespace API.Mappings;
public class BookMarkMcqConverter : ITypeConverter<Mcq, Task<McqResponse>>
{
    private readonly IMcqService _McqService;

    public BookMarkMcqConverter(IMcqService McqService)
    {
        _McqService = McqService;
    }
    public async Task<McqResponse> Convert(Mcq source, Task<McqResponse> destination, ResolutionContext context)
    {
        return new McqResponse {
            IsBookedmarked = await _McqService.IsBookedmarked(source.Id),
            Id = source.Id,
            TargetId = source.Target.Id,
            AllowRandom = source.AllowRandom,
            AnswerOptions = source.AnswerOptions,
            Explanation = source.Explanation,
            QuestionText = source.QuestionText,
        };             
    }
}
