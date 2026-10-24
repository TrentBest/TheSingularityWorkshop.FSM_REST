# FSM_REST Domain Gap Assessment

**Owner / Workshop moniker:** @TrentBest  
**Branch:** development  
**Purpose:** Separate implemented behavior from the remaining domain before adding more features.  
**Assessment basis:** Production sources, current domain statement, and current test source files on the development branch. This is a working audit, not a claim of completion.

## Executive finding

FSM_REST is currently a useful REST capability prototype, not a complete general-purpose REST integration layer. It has descriptors, string-valued parameter binding, request construction, an HttpClient transport, response capture/deserialization, and optional FSM_COS/GUI adapters. The supported OpenAPI JSON importer and definition-URL connection now live in a separate optional `TheSingularityWorkshop.FSM_Rest.OpenApi` project/package. Existing tests exercise important happy paths and selected failures, but do not yet demonstrate comprehensive REST semantics, robust malformed-input handling, broad protocol behavior, or cross-layer integration.

The earlier **20/20** figure referred to production C# files having companion Markdown documentation. It was not a unit-test count. Documentation coverage must not be used as a proxy for behavioral coverage.

## Current implementation inventory

| Capability | Current state | Domain gap / consequence |
|---|---|---|
| API and operation descriptors | Implemented | Records accept malformed/null/inconsistent data; collection immutability and validation guarantees need explicit decisions/tests. |
| Runtime operation binding | Implemented | String-only values; no typed values or explicit duplicate/case policy beyond current dictionary behavior. |
| Path/query/header/cookie binding | Implemented for scalar strings | No array/object serialization, style/explode rules, nullable/empty-value policy, reserved-character policy, or comprehensive URI edge coverage. |
| Request body | Optional text body and content type | No typed body serialization, body schema validation, multipart/form-data, or binary/streaming body contract. These may belong in optional providers, but the extension boundary needs to be explicit. |
| Transport abstraction | Async send with cancellation | No streaming response contract, configurable completion/body buffering policy, timeout/retry policy, or standard middleware/interceptor seam. Some are optional policy adapters rather than core requirements. |
| HttpClient adapter | Sends request and captures headers/body | Needs broader tests for unusual methods, content headers, repeated headers, empty/no-content responses, redirects/URI behavior, cancellation, transport exceptions, and disposal/ownership contract. |
| Response | Status, headers, body text, JSON deserialization | No typed result/error mapping, schema validation, media-type-aware deserialization, streaming, or standardized problem-details handling. Keep these composable; decide which belongs in core versus adapters. |
| OpenAPI JSON import | Operation-level subset; local references and warnings | Not full OpenAPI conformance. The importer was moved into the optional OpenAPI project to enforce the format-specific provider boundary. Reconcile this boundary rather than silently expanding core. |
| OpenAPI fidelity | Method/path/parameters/request-body/response metadata subset | Needs explicit tests and warnings for parameter serialization, security, server selection, callbacks/webhooks, composed schemas, external references, and other unsupported constructs. |
| FSM_COS MicroBundle | Adapter exposes API capability | Needs contract-level tests for identity, provider/operation mapping, metadata preservation, invalid descriptors, and compatibility with the current MicroBundle contract. |
| GUI adapter | Maps operations to GUI nodes / executes mapped operations | Needs behavior and failure-path tests for all input types, missing/invalid values, mapping collisions, execution errors, and generated metadata; ensure optional adapter boundaries stay optional. |
| Package/release compatibility | Core targets net8.0 today | Ecosystem target-framework compatibility is not proven merely by a successful build. Decide and test supported TFMs before release. |

## Domain boundary decisions to preserve

- Core remains framework-neutral and has no dependency on FSM_COS, GUI, Unity, or a concrete service.
- IRestTransport remains the replaceable network boundary; request construction must be testable without network traffic.
- Authentication, credential discovery, retries, caching, rate limiting, telemetry, service discovery, and server hosting are policy/integration concerns. They should be separate optional components unless a concrete core contract is justified.
- OpenAPI and other description-format importers should be provider/adapter capabilities. OpenAPI-specific code now lives in `src/FSM_REST.OpenApi`, a separate optional project/package that depends on the neutral core. Keep future format-specific providers outside core.
- NuGet is code delivery, not a MicroBundle registry. The REST MicroBundle represents capability/instructions, not stored response data.

## Implementation sequence

### P0 — correctness and contract tests
1. Add a domain-focused test matrix for descriptor invariants, binding rules, URI composition/escaping, headers/cookies, body/content type, HTTP status/headers/body, cancellation, and exception propagation.
2. Fix discovered correctness defects with regression tests, not just test-count growth.
3. Define and enforce path/method/parameter validation; test malformed paths and URI boundary cases.
4. Test cancellation and transport errors without live network access; test 204/empty-body and non-2xx behavior.

### P1 — REST parameter semantics
1. Decide the supported parameter value model (scalar-only versus typed/collection values).
2. If general REST/OpenAPI endpoint coverage is required, add explicit style/explode and serialization support or reject unsupported forms with actionable diagnostics. Do not silently flatten arrays/objects.
3. Define empty versus missing values, repeated query keys, case sensitivity, header validation, and cookie merge behavior.
4. Add table-driven tests for reserved characters, Unicode, spaces, slashes, plus signs, percent signs, and empty values.

### P1 — response and body contracts
1. Specify whether core responses are always buffered text or can support binary/streaming payloads.
2. Specify request-body content types and whether JSON serialization belongs in an optional serializer/provider.
3. Add tests for JSON null, empty body, malformed JSON, non-JSON media types, response headers, and HTTP error payloads.

### P1 — adapter boundaries and fidelity
1. Reconcile OpenAPI importer placement with the neutral-core boundary.
2. Build fixture-driven OpenAPI tests for path-level/operation-level parameter overrides, local/external/cyclic/missing references, security warnings, request bodies, responses, and unsupported constructs.
3. Test FSM_COS and GUI adapters independently at their contracts and ensure the core package does not acquire their dependencies.

### P2 — release proof
1. Establish supported target frameworks from actual ecosystem consumers.
2. Add package-content/metadata checks and consumer smoke tests for each supported target.
3. Publish no package until explicitly approved; CI must prove build, tests, package validation, and adapter compatibility at the same commit.

## Completion evidence required

A test count alone is not a completion criterion. For each capability, record:
- public contract and invariants;
- positive, negative, boundary, and adversarial test cases;
- whether the test uses a deterministic fake or an external service;
- known limitations and unsupported semantics;
- exact commit and CI run that validates the change.

**Status: architecture boundary improved by isolating OpenAPI; domain completeness remains unproven and requires end-to-end tests and consumer evidence.**