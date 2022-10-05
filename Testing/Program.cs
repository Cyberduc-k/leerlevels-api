using Data;
using Microsoft.EntityFrameworkCore;

DbContextOptions<TargetContext> contextOptions = new DbContextOptionsBuilder<TargetContext>()
    .UseInMemoryDatabase("TestDatabase")
    .Options;

using TargetContext context = new(contextOptions);

context.Database.EnsureCreated();

foreach (Model.AnswerOption answer in context.AnswerOptions) {
    Console.WriteLine($"{answer.Id}: {answer.Index}, {answer.Text}, {answer.IsCorrect}");
}

Console.WriteLine();

foreach (Model.Mcq mcq in context.Mcqs) {
    Console.WriteLine($"{mcq.Id}: {mcq.QuestionText}, {mcq.Explanation}, {mcq.AllowRandom}");
}

Console.WriteLine();

foreach (Model.Target target in context.Targets) {
    Console.WriteLine($"{target.Id}: {target.Label}, {target.Description}, {target.YoutubeId}, {target.ImageUrl}");
}