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

    [Fact]
    public void HeaderValuesAreCopiedAndReadOnly()
    {
        var sourceValues = new List<string> { "session=abc; Path=/", "preference=dark; Path=/" };
        var source = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Set-Cookie"] = sourceValues
        };

        var response = new RestResponse(200, "OK", new Dictionary<string, string>(), "ok")
        {
            HeaderValues = source
        };

        sourceValues[0] = "changed";
        source["Set-Cookie"] = new[] { "replaced" };

        Assert.Equal(
            new[] { "session=abc; Path=/", "preference=dark; Path=/" },
            response.HeaderValues["set-cookie"]);
        Assert.Throws<NotSupportedException>(() =>
            ((IDictionary<string, IReadOnlyList<string>>)response.HeaderValues)["X-Test"] = new[] { "value" });
        Assert.Throws<NotSupportedException>(() =>
            ((IList<string>)response.HeaderValues["Set-Cookie"])[0] = "changed");
    }

    [Fact]
    public void HeaderValuesRejectsNullValueCollections()
    {
        var source = new Dictionary<string, IReadOnlyList<string>>
        {
            ["Set-Cookie"] = null!
        };

        Assert.Throws<ArgumentNullException>(() =>
            new RestResponse(200, "OK", new Dictionary<string, string>(), "ok")
            {
                HeaderValues = source
            });
    }

    [Fact]
    public void HeaderValuesRejectsCaseInsensitiveDuplicateNames()
    {
        var source = new Dictionary<string, IReadOnlyList<string>>
        {
            ["X-Trace"] = new[] { "first" },
            ["x-trace"] = new[] { "second" }
        };

        Assert.Throws<ArgumentException>(() =>
            new RestResponse(200, "OK", new Dictionary<string, string>(), "ok")
            {
                HeaderValues = source
            });
    }
}
