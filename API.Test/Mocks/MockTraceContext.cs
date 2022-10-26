using Microsoft.Azure.Functions.Worker;

namespace API.Test.Mocks;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockTraceContext.cs
public class MockTraceContext : TraceContext
{
    public MockTraceContext(string traceParent, string traceState)
    {
        TraceParent = traceParent;
        TraceState = traceState;
    }

    public override string TraceParent { get; }
    public override string TraceState { get; }
}
