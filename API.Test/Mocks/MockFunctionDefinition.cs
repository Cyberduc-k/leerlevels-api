using System.Collections.Immutable;
using Microsoft.Azure.Functions.Worker;

namespace API.Test.Mocks;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockFunctionDefinition.cs
public class MockFunctionDefinition : FunctionDefinition
{
    public static readonly string DefaultPathToAssembly = typeof(MockFunctionDefinition).Assembly.Location;
    public static readonly string DefaultEntrypPoint = $"{nameof(MockFunctionDefinition)}.{nameof(DefaultEntrypPoint)}";
    public static readonly string DefaultId = "TestId";
    public static readonly string DefaultName = "TestName";

    public MockFunctionDefinition(
        string? functionId = null,
        IDictionary<string, BindingMetadata>? inputBindings = null,
        IDictionary<string, BindingMetadata>? outputBindings = null,
        IEnumerable<FunctionParameter>? parameters = null)
    {
        if (!string.IsNullOrWhiteSpace(functionId)) {
            Id = functionId;
        }

        Parameters = parameters?.ToImmutableArray() ?? ImmutableArray<FunctionParameter>.Empty;
        InputBindings = inputBindings?.ToImmutableDictionary() ?? ImmutableDictionary<string, BindingMetadata>.Empty;
        OutputBindings = outputBindings?.ToImmutableDictionary() ?? ImmutableDictionary<string, BindingMetadata>.Empty;
    }

    public override ImmutableArray<FunctionParameter> Parameters { get; }
    public override string PathToAssembly { get; } = DefaultPathToAssembly;
    public override string EntryPoint { get; } = DefaultEntrypPoint;
    public override string Id { get; } = DefaultId;
    public override string Name { get; } = DefaultName;
    public override IImmutableDictionary<string, BindingMetadata> InputBindings { get; }
    public override IImmutableDictionary<string, BindingMetadata> OutputBindings { get; }

    public static FunctionDefinition Generate(int inputBindingCount = 0, int outputBindingCount = 0, params Type[] paramTypes)
    {
        Dictionary<string, BindingMetadata> inputs = new();
        Dictionary<string, BindingMetadata> outputs = new();
        List<FunctionParameter> parameters = new();

        inputs.Add("triggerName", new MockBindingMetadata("triggerName", "TestTrigger", BindingDirection.In));

        for (int i = 0; i < inputBindingCount; i++) {
            inputs.Add($"inputName{i}", new MockBindingMetadata($"inputName{i}", $"TestInput{i}", BindingDirection.In));
        }

        for (int i = 0; i < outputBindingCount; i++) {
            outputs.Add($"outputName{i}", new MockBindingMetadata($"outputName{i}", $"TestOutput{i}", BindingDirection.Out));
        }

        for (int i = 0; i < paramTypes.Length; i++) {
            Dictionary<string, object> properties = new() {
                { "TestPropertyKey", "TestPropertyValue" }
            };

            parameters.Add(new FunctionParameter($"Parameter{i}", paramTypes[i], properties.ToImmutableDictionary()));
        }

        return new MockFunctionDefinition(null, inputs, outputs, parameters);
    }
}
