# FSM_REST Domain Gap Assessment

**Owner / Workshop moniker:** @TrentBest  
**Assessment scope:** current development baseline plus the proposed changes in [PR #14](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/pull/14).  
**Purpose:** distinguish implemented behavior from missing or unverified domain capability. This is an evidence-based working audit, not a claim of full REST/OpenAPI conformance.

## Executive finding

FSM_REST has a useful protocol-neutral REST capability foundation: descriptors, scalar string binding, request construction, a replaceable transport seam, an HttpClient adapter, response capture, and optional FSM_COS, GUI, and OpenAPI projects. PR #14 adds fluent HTTP transport construction, explicit ownership/disposal, named `IHttpClientFactory` integration, a reusable factory, and optional DI registration. The PR's full CI run passed on head commit `2dcec9ad3d0872a1e9f81a27b44107d163c2c407` ([run 40576237860](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40576237860)); the PR remains unmerged.

Passing CI proves the current tested behavior, not comprehensive protocol support. The previous **20/20** figure referred to production C# files with companion Markdown pages, not a unit-test count. Documentation coverage and test count are not proxies for domain completeness.

Track implementation work in [issue #15 — Domain completeness backlog](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/issues/15). The existing [issue #13](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/issues/13) remains the focused decision thread for Workshop HTTP ownership and streaming/binary semantics.

## Current implementation inventory

| Capability | Current state | Remaining gap / consequence |
|---|---|---|
| API and operation descriptors | Implemented | Decide and test validation for null, malformed, contradictory, or mutable collection inputs; avoid accidental assumptions about immutability. |
| Runtime operation binding | Implemented | Scalar string values only. Missing/null and explicit-empty behavior is deliberate where representable. Typed values and repeated/collection values are not supported by the current dictionary model. |
| Path/query/header/cookie binding | Implemented for scalar strings | Existing tests cover important escaping and ambiguity cases. Continue URI normalization, malformed/encoded path, header edge-case, and OpenAPI parameter-override tests. Arrays/objects and full style/explode semantics remain unsupported. |
| Request body | Optional text body and content type | No streaming/binary request body, multipart contract, or core typed-body serialization/schema validation. Decide whether reusable extensions belong in optional providers. |
| Transport contract | `IRestTransport.SendAsync` with cancellation | No stream-oriented request/response contract or explicit middleware/interceptor seam. Add only where justified by concrete consumer needs. |
| HttpClient adapter | Sends request and captures status, headers, and body text; caller-owned clients are not disposed by direct construction | PR #14 adds explicit ownership for builder/factory-created clients, injected-handler lifetime behavior, and disposal tests. Remaining edge coverage includes repeated response headers, unusual valid methods, redirects/URI behavior, cancellation timing, and concurrent disposal/send semantics. |
| Builder/factory/DI | Implemented in PR #14; CI passed | PR remains unmerged. Builder is single-use; `ConfigureClient` is restricted to builder-created clients; caller-supplied clients remain caller-owned; named factory clients are supported. Consider further APIs only if a real use case is demonstrated. |
| Response | Status, reason phrase, headers, text body, and JSON deserialization helper | No streaming, schema validation, media-type-aware typed results, standardized problem-details interpretation, or reusable typed transport-error model. Decide which belong in core versus adapters. |
| OpenAPI JSON import | Supported subset in optional `TheSingularityWorkshop.FSM_Rest.OpenApi` package | Not full OpenAPI conformance. Need fixture-driven fidelity tests and actionable diagnostics for unsupported constructs. |
| FSM_COS MicroBundle | Optional adapter exposes REST capability | Expand contract tests for identity, operation/provider mapping, metadata preservation, invalid descriptors, and MicroBundle compatibility. |
| GUI adapter | Optional mapping/execution path | Expand supported-input, missing/invalid-value, collision, execution-failure, result-publication, and metadata tests. Keep renderer and GUI policy outside core. |
| Foundry / workflow boundary | FSM_REST is a capability and transport substrate | Verify host integration while keeping workflow sequencing/outcomes owned by Foundry; see [issue #12](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/issues/12). |
| Package/release compatibility | Core currently targets `net8.0`; pack and package audit pass in PR #14 CI | Establish target frameworks from real consumers; test package contents, dependency graphs, source/package parity, and optional-package smoke tests before release. |

## Domain boundaries to preserve

- Core remains framework-neutral: no FSM_COS, GUI, Unity, or concrete business-service dependency.
- `IRestTransport` remains the one shared runtime transport boundary. Builder and factory are construction helpers, not competing transport abstractions.
- A descriptor is reusable capability metadata; a binding is one invocation; request construction does not send network traffic.
- A transport reports an observed response. An HTTP non-2xx status is still a response, not automatically a transport exception or workflow failure.
- Authentication, credentials, retries, caching, rate limits, telemetry, service discovery, and server hosting are host/policy or optional-adapter concerns unless a concrete shared contract is justified.
- OpenAPI and future description-format importers stay in provider/adapter packages. Core must not silently imply that unsupported source-format semantics are represented.
- NuGet is code delivery, not a MicroBundle registry. A REST MicroBundle describes a capability; it does not store the remote response dataset.

## Remaining implementation sequence

### P0 — Core correctness and contract proof

1. Review descriptor constructors/records for null, malformed, contradictory, and mutable inputs; define public invariants and add boundary tests.
2. Complete deterministic request-construction coverage for base-URI joining, malformed/encoded paths, encoded separators, fragments, query delimiters, Unicode, duplicate names, and parameter precedence where applicable.
3. Complete HTTP adapter edge coverage for repeated headers, casing/combination, content headers, unusual valid methods, redirects, empty/error bodies, cancellation timing, and exception propagation.
4. Define disposal/concurrency guarantees. Test repeated/concurrent sends, disposal races, cancellation before/during send, and idempotent disposal.
5. Test JSON null, empty body, malformed JSON, non-JSON media types, and large text responses. Fix defects with regression tests rather than increasing test count alone.

### P1 — Payload and transport contracts

1. Resolve the streaming/binary design against actual consumer requirements in issue #13: direction, stream ownership/disposal, cancellation, status/headers, partial reads, bounded-memory behavior, and compatibility with the existing string contract.
2. Only after the contract is chosen, add deterministic streaming/binary adapter tests. Do not silently reinterpret `RestRequest.Body` or `RestResponse.Body`.
3. Decide the core/provider boundary for JSON request serialization, typed bodies, multipart/form-data, content negotiation, media-type-aware response handling, and structured transport errors.
4. Preserve the distinction between HTTP error responses and exceptions raised because a request could not be transported.
5. Do not add auth/retry/cache/telemetry middleware merely for breadth; evaluate it when real consumers establish a reusable contract.

### P1 — OpenAPI fidelity and adapter boundaries

1. Add fixture-driven OpenAPI tests for path/operation parameter overrides, missing/invalid/cyclic/local references, external references, security schemes/requirements, request bodies, responses, and unsupported constructs.
2. Verify unsupported arrays/objects, style/explode, server variables, composed schemas, callbacks/webhooks, and security constructs produce actionable warnings or errors rather than implied support.
3. Publish a supported-subset matrix and test package dependency boundaries so format-specific code stays out of core.
4. Test FSM_COS and GUI adapters independently; add Foundry execution-boundary and descriptor serialization/reflection round-trip proofs where actual consumers require them.

### P2 — Release and ecosystem proof

1. Derive supported target frameworks from actual Workshop consumers and test them rather than assuming the current target is sufficient.
2. Add package-content/metadata checks and consumer smoke tests for core and optional packages, including dependency graphs and accidental dependency exclusions.
3. Verify NuGet IDs/versions and source/package API parity before an explicitly authorized release.
4. Keep package packing and auditing in CI; keep publishing gated until explicit owner authorization.
5. Check documentation and examples against source and CI evidence. Mark live-service behavior as unverified unless it was actually exercised.

## Completion evidence

For each capability, record (1) the public contract and invariants, (2) positive/negative/boundary/adversarial tests, (3) deterministic fake versus external-service evidence, (4) known unsupported semantics, and (5) exact commit and CI run. A checkbox is complete only when evidence supports it.

**Current status:** the HTTP construction domain is implemented and CI-verified in PR #14, pending review/merge. Full REST capability completeness remains unproven; use [issue #15](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/issues/15) as the prioritized TODO list. No publication is authorized.