using Microsoft.Azure.Functions.Worker;

namespace API.Test.Mocks;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockBindingMetadata.cs
public class MockBindingMetadata : BindingMetadata
{
    public MockBindingMetadata(string name, string type, BindingDirection direction)
    {
        Name = name;
        Type = type;
        Direction = direction;
    }

    public override string Name { get; }
    public override string Type { get; }
    public override BindingDirection Direction { get; }
}
