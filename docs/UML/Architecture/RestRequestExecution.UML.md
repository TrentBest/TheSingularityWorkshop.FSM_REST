<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Request Execution UML

**Target:** Request construction and transport execution.  
**Provenance:** Source-grounded overview, manually maintained and not generated.  
**Source:** [Request construction](../../../src/FSM_REST/RestRequestFactory.cs), [transport contract](../../../src/FSM_REST/IRestTransport.cs), [HTTP adapter](../../../src/FSM_REST/HttpClientRestTransport.cs), [response envelope](../../../src/FSM_REST/RestResponse.cs).

## Execution path

```mermaid
flowchart TD
    Descriptor[RestOperationDescriptor] --> Binding[RestOperationBinding]
    Binding --> Factory[RestRequestFactory]
    Factory --> Request[RestRequest]
    Request --> Transport[IRestTransport]
    Transport --> Response[RestResponse]
    Http[HttpClientRestTransport] -. implements .-> Transport
    Host[Host / caller policy] --> Transport
    Host --> Response
```

## Responsibilities and invariants

- **`RestOperationDescriptor`** describes a reusable operation, not a request in flight.
- **`RestOperationBinding`** pairs the operation with values for one invocation.
- **`RestRequestFactory`** validates and composes a request; it performs no network I/O.
- **`IRestTransport`** defines the asynchronous send boundary and accepts a `CancellationToken`.
- **`HttpClientRestTransport`** adapts the contract to a host-supplied `HttpClient`.
- **`RestResponse`** preserves observed status, headers, and body. A non-2xx response is not automatically a transport exception.

## Use cases

### Construct without sending
Bind values and call `RestRequestFactory.Create`. This path is testable without networking.

### Substitute transport
A test or host can provide another `IRestTransport` without changing descriptors or request construction.

### Send and inspect
Call `SendAsync` with a cancellation token and inspect the returned `RestResponse`. Authentication, retries, payload interpretation, and other application policies remain explicit consumer concerns.

## Implementation and tests

- [Request factory](../../../src/FSM_REST/RestRequestFactory.cs)
- [Request contract](../../../src/FSM_REST/RestRequest.cs)
- [Transport contract](../../../src/FSM_REST/IRestTransport.cs)
- [HTTP adapter](../../../src/FSM_REST/HttpClientRestTransport.cs)
- [Response envelope](../../../src/FSM_REST/RestResponse.cs)
- [Transport tests](../../../tests/FSM_REST.Tests/RestTransportTests.cs)

This diagram explains boundaries; it does not substitute for test and CI evidence.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
