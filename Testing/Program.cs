using System.Text.Json;
using Data;
using Microsoft.EntityFrameworkCore;
using Model;

DbContextOptions<TargetContext> contextOptions = new DbContextOptionsBuilder<TargetContext>()
    .UseInMemoryDatabase("TestDatabase")
    .Options;

using TargetContext context = new(contextOptions);

context.Database.EnsureCreated();

object mapMcq(Mcq m) => new { m.Id, TargetId = m.Target.Id, m.QuestionText, m.Explanation, m.AllowRandom, m.AnswerOptions };
object mapTarget(Target t) => new { t.Id, t.Label, t.Description, t.TargetExplanation, t.YoutubeId, t.ImageUrl, Mcqs = t.Mcqs.Select(mapMcq) };

IEnumerable<object> targets = context.Targets.Include(t => t.Mcqs).ThenInclude(m => m.AnswerOptions).Select(mapTarget);

foreach (object target in targets) {
    string json = JsonSerializer.Serialize(target, new JsonSerializerOptions { WriteIndented = true });

    Console.WriteLine(json);
}