using TheSingularityWorkshop.FSM_REST;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestFsmExecutorTests
{
    private static readonly RestRequest Request =
        new("GET", new Uri("https://example.test/items"));

    [Fact]
    public async Task SendAsync_ReturnsNonSuccessHttpResponseAsCompletedTransportResult()
    {
        var expected = new RestResponse(
            404,
            "Not Found",
            new Dictionary<string, string>(),
            "missing");
        var transport = new DelegateTransport((_, _) => Task.FromResult(expected));

        var actual = await RestFsmExecutor.SendAsync(Request, transport);

        Assert.Same(expected, actual);
        Assert.False(actual.IsSuccessStatusCode);
    }

    [Fact]
    public async Task SendAsync_RethrowsTransportFailure()
    {
        var expected = new HttpRequestException("controlled transport failure");
        var transport = new DelegateTransport((_, _) => Task.FromException<RestResponse>(expected));

        var actual = await Assert.ThrowsAsync<HttpRequestException>(
            () => RestFsmExecutor.SendAsync(Request, transport));

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task SendAsync_RethrowsRequestedCancellation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var transport = new DelegateTransport((_, token) =>
            Task.FromCanceled<RestResponse>(token));

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => RestFsmExecutor.SendAsync(Request, transport, cancellation.Token));
    }

    [Fact]
    public async Task SendAsync_RethrowsUnrequestedCancellationAsTransportFailure()
    {
        var expected = new OperationCanceledException("transport cancellation without caller cancellation");
        var transport = new DelegateTransport((_, _) =>
            Task.FromException<RestResponse>(expected));

        var actual = await Assert.ThrowsAsync<OperationCanceledException>(
            () => RestFsmExecutor.SendAsync(Request, transport));

        Assert.Same(expected, actual);
    }


    [Fact]
    public async Task SendAsync_ConcurrentInvocationsKeepTheirResponsesIndependent()
    {
        var firstResponse = new RestResponse(
            200,
            "OK",
            new Dictionary<string, string>(),
            "first");
        var secondResponse = new RestResponse(
            201,
            "Created",
            new Dictionary<string, string>(),
            "second");
        var firstStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSecond = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var firstTransport = new DelegateTransport(async (_, _) =>
        {
            firstStarted.TrySetResult();
            await releaseFirst.Task;
            return firstResponse;
        });
        var secondTransport = new DelegateTransport(async (_, _) =>
        {
            secondStarted.TrySetResult();
            await releaseSecond.Task;
            return secondResponse;
        });

        var firstTask = RestFsmExecutor.SendAsync(Request, firstTransport);
        var secondTask = RestFsmExecutor.SendAsync(Request, secondTransport);

        await Task.WhenAll(firstStarted.Task, secondStarted.Task);
        releaseFirst.TrySetResult();
        Assert.Same(firstResponse, await firstTask);

        releaseSecond.TrySetResult();
        Assert.Same(secondResponse, await secondTask);
    }

    private sealed class DelegateTransport(
        Func<RestRequest, CancellationToken, Task<RestResponse>> send) : IRestTransport
    {
        public Task<RestResponse> SendAsync(
            RestRequest request,
            CancellationToken cancellationToken = default) =>
            send(request, cancellationToken);
    }
}
