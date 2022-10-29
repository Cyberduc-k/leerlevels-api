using Microsoft.Azure.Functions.Worker;

namespace API.Test.Mock;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockFunctionInvocation.cs
public class MockFunctionInvocation : FunctionInvocation
{
    public MockFunctionInvocation(string id = "", string functionId = "")
    {
        if (!string.IsNullOrWhiteSpace(id)) Id = id;
        if (!string.IsNullOrWhiteSpace(functionId)) FunctionId = functionId;
    }

    public override string Id { get; } = Guid.NewGuid().ToString();
    public override string FunctionId { get; } = Guid.NewGuid().ToString();
    public override TraceContext TraceContext { get; } = new MockTraceContext(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
}
