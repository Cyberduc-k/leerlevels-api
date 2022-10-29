using Microsoft.Azure.Functions.Worker;

namespace API.Test.Mock;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockFunctionContext.cs
public class MockFunctionContext : FunctionContext, IDisposable
{
    private readonly FunctionInvocation _invocation;

    public MockFunctionContext() : this(new MockFunctionDefinition(), new MockFunctionInvocation())
    {
    }

    public MockFunctionContext(FunctionDefinition functionDefinition, FunctionInvocation invocation)
    {
        FunctionDefinition = functionDefinition;
        _invocation = invocation;
    }

    public override string InvocationId => _invocation.Id;
    public override string FunctionId => _invocation.FunctionId;
    public override TraceContext TraceContext => _invocation.TraceContext;
    public override BindingContext BindingContext { get; }
    public override RetryContext RetryContext { get; }
    public override IServiceProvider InstanceServices { get; set; }
    public override FunctionDefinition FunctionDefinition { get; }
    public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();
    public override IInvocationFeatures Features { get; }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}
