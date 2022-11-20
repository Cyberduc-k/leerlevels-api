using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Model;
using Model.Response;
using Service;
using Service.Interfaces;

namespace API.Mappings;
public class BookMarkTargetConverter : ITypeConverter<Target, Task<TargetResponse>> 
{
    private readonly IBookmarkService _bookMarkedService;

    public BookMarkTargetConverter(IBookmarkService bookmarkedService)
    {
        _bookMarkedService = bookmarkedService;
    }
    public async Task<TargetResponse> Convert(Target source, Task<TargetResponse> destination, ResolutionContext context)
    {
        ICollection< McqResponse> mcqResponses = new List< McqResponse>();  
        McqResponse mcqResponse = new McqResponse();
        foreach (var item in source.Mcqs) {
            mcqResponse.AnswerOptions = item.AnswerOptions;
            mcqResponse.QuestionText = item.QuestionText;
            mcqResponse.AllowRandom = item.AllowRandom;
            mcqResponse.Explanation = item.Explanation;
        }
        mcqResponses.Add(mcqResponse);

        return new() {
           IsBookedmarked = await _bookMarkedService.IsBookedmarked(source.Id),
            Id = source.Id,
            TargetExplanation = source.TargetExplanation,
            Description = source.Description,
            ImageUrl = source.ImageUrl,
            Label = source.Label,
            YoutubeId = source.YoutubeId,
            Mcqs = mcqResponses,
        };

    }
}
