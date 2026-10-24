# FSM_REST Request Execution UML

****Provenance:** Source-grounded execution overview; manually maintained, not generated.
**Source:** [Request construction](../../src/FSM_REST/RestRequestFactory.cs) · [Transport contract](../../src/FSM_REST/IRestTransport.cs) · [HTTP adapter](../../src/FSM_REST/HttpClientRestTransport.cs)

uthor / workshop moniker:** @TrentBest  
**Purpose:** Explain the implementation boundary between describing a REST operation, constructing a concrete request, sending it, and interpreting the result.

## Class card

```text
+---------------------------+
| RestOperationDescriptor   |
+---------------------------+
| method, path, parameters   |
| body/response metadata    |
+-------------+-------------+
              |
              | operation + invocation values
              v
+---------------------------+       +----------------------+
| RestOperationBinding      |------>| RestRequestFactory   |
+---------------------------+       +----------+-----------+
| Operation                 |                  |
| Parameters                |                  | creates
+---------------------------+                  v
                                     +----------------------+
                                     | RestRequest          |
                                     +----------+-----------+
                                                |
                                                | SendAsync(request, token)
                                                v
                                     +----------------------+
                                     | IRestTransport       |
                                     +----------+-----------+
                                                ^
                                                | implements
                                     +----------------------+
                                     | HttpClientRestTransport
                                     +----------+-----------+
                                                |
                                                | HttpClient
                                                v
                                     +----------------------+
                                     | RestResponse         |
                                     +----------------------+
                                     | status, headers, body|
                                     +----------------------+
```

## Responsibilities and invariants

- **`RestOperationDescriptor`** describes a reusable operation; it is not a request in flight.
- **`RestOperationBinding`** pairs an operation with values for one invocation.
- **`RestRequestFactory`** validates and composes a request. Calling it does not perform network I/O.
- **`RestRequest`** is a transport-neutral request envelope. It does not own connection lifetime or retry policy.
- **`IRestTransport`** defines the asynchronous send boundary and accepts a `CancellationToken`.
- **`HttpClientRestTransport`** adapts that contract to a host-supplied `HttpClient`; it does not own or configure that client.
- **`RestResponse`** preserves the observed status, headers, and body. `IsSuccessStatusCode` only classifies the 2xx range; it does not interpret the payload.

## Use cases

### Construct without sending

A caller binds parameter values and calls `RestRequestFactory.Create`. This step can be tested independently from networking and should reject missing required parameters and non-absolute base URIs.

### Substitute transport

A test or host can implement `IRestTransport` without changing operation descriptors or request construction. The supplied `HttpClientRestTransport` is one adapter, not the only permitted implementation.

### Send and inspect

The caller invokes `SendAsync` with a cancellation token and receives a `RestResponse`. Non-2xx responses are represented as responses rather than automatically converted into exceptions by this adapter. Payload interpretation, retry decisions, authentication, and application policy remain with the caller or another explicit layer.

## Implementation and tests

- Request construction: [`RestRequestFactory.cs`](../../src/FSM_REST/RestRequestFactory.cs)
- Request contract: [`RestRequest.cs`](../../src/FSM_REST/RestRequest.cs)
- Transport contract: [`IRestTransport.cs`](../../src/FSM_REST/IRestTransport.cs)
- HTTP adapter: [`HttpClientRestTransport.cs`](../../src/FSM_REST/HttpClientRestTransport.cs)
- Response envelope: [`RestResponse.cs`](../../src/FSM_REST/RestResponse.cs)
- Transport tests: [`RestTransportTests.cs`](../../tests/FSM_REST.Tests/RestTransportTests.cs)

The tests cover request/response mapping, preservation of non-success status, cancellation, and null-argument guards. Consult CI before treating any branch as verified.
