<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

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

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
