# REST capability end-to-end demo

**Workshop moniker:** @TrentBest  
**Purpose:** Provide a deterministic, runnable proof of the neutral REST core's principal execution path.

## What it demonstrates
- Describes one operation and its path parameter.
- Binds runtime values separately from the reusable descriptor.
- Constructs a request without network I/O.
- Sends through `IRestTransport` using `HttpClientRestTransport`.
- Receives and checks a response from a controlled in-memory HTTP handler.

## Run
From the repository root:

```bash
dotnet run --project samples/RestCapabilityDemo/RestCapabilityDemo.csproj --configuration Release
```

The handler accepts only the expected GET URL and returns a deterministic JSON payload. The sample requires no live service, credentials, or network access. It verifies the core path, not OpenAPI conformance or production-host policy.

## Source and architecture
- Source: [Program.cs](Program.cs)
- Core contract: [Architecture Contract](../../docs/ARCHITECTURE.md)
- Detailed consumer guide: [CONSUMING.md](../../docs/CONSUMING.md)
- Gallery: [Gallery.md](../../Gallery.md)
