using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestResponseTests
{
    private sealed record Person(int Id, string Name);

    [Fact]
    public void DeserializesJsonBodyIntoConsumerOwnedModel()
    {
        var response = new RestResponse(
            200,
            "OK",
            new Dictionary<string, string> { ["content-type"] = "application/json" },
            """{"id":42,"name":"Ada"}""");

        var person = response.Deserialize<Person>();

        Assert.NotNull(person);
        Assert.Equal(42, person.Id);
        Assert.Equal("Ada", person.Name);
    }

    [Fact]
    public void ThrowsJsonExceptionForMalformedJson()
    {
        var response = new RestResponse(200, "OK", new Dictionary<string, string>(), "{");
        Assert.Throws<System.Text.Json.JsonException>(() => response.Deserialize<Person>());
    }
}
