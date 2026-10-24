# FSM_REST Release Notes

**Owner / Workshop moniker:** @TrentBest  
**Status:** Pre-release notes for the current candidate line; publication is not authorized by this document.

## Current candidate package set

| Package | Candidate version | Role |
|---|---|---|
| `TheSingularityWorkshop.FSM_Rest` | `0.1.0-alpha.5` | Framework-neutral REST capability descriptors, parameter binding, request construction, transport abstraction, and response capture. |
| `TheSingularityWorkshop.FSM_Rest.OpenApi` | `0.1.0-alpha.1` | Optional OpenAPI 3.x JSON import and definition-URL connection adapter. |
| `TheSingularityWorkshop.FSM_Rest.COS` | `0.1.0-alpha.1` | Optional FSM_COS MicroBundle integration for reusable REST capability descriptors. |

These are prerelease versions. Treat APIs and package contracts as subject to change before a stable release. This document describes the current candidate line; the exact commit, CI run, and package artifact must be recorded in the release checklist for each release candidate.

## Core alpha 5

- Adds a direct dependency on `TheSingularityWorkshop.FSM_API` `1.0.13` and an opt-in FSM-governed REST transport lifecycle.
- Keeps the core package independent of FSM_COS, GUI frameworks, and protocol-description importers.
- Provides descriptors for APIs and operations, string-valued operation binding, request construction, an `IRestTransport` boundary, an `HttpClient` adapter, and response capture/deserialization.

## Optional companion packages

- **OpenAPI alpha 1:** provides an initial OpenAPI 3.x JSON subset importer and a definition-URL connection adapter. It is not a complete OpenAPI implementation.
- **COS alpha 1:** provides the optional adapter for representing REST capability metadata as FSM_COS MicroBundle content. It depends on the core package and MicroBundleDomain.
- GUI-related adapters remain optional integration surfaces; the core package does not render controls or impose a host state container.

## Compatibility and dependency boundaries

- Current candidate packages target **.NET 8.0**. Compatibility with other target frameworks has not been established by this candidate's build.
- Core depends on FSM_API `1.0.13`; it must not acquire a MicroBundleDomain or FSM_COS dependency.
- OpenAPI import and FSM_COS integration are separate companion packages. Keep format-specific and composition-specific dependencies out of core.
- Consumers should install only the companion packages needed for their integration.

## Known limitations

This candidate is a REST capability and transport prototype, not a complete general-purpose REST integration layer. In particular:

- Operation parameters are currently represented as scalar strings; comprehensive array/object serialization and OpenAPI style/explode semantics are not implemented.
- The OpenAPI importer supports an initial JSON subset and supported local component references. It is not full OpenAPI conformance; external references, server-variable substitution, full schema composition/validation, and several protocol features remain unsupported.
- Authentication, credential discovery, retries, caching, rate limiting, service discovery, and authorization policy belong to the host or separate optional adapters.
- Responses are captured as body text with convenience JSON deserialization. Schema validation, standardized domain-error mapping, and a general streaming/binary response contract are not provided.
- Existing tests cover useful paths and selected failures but do not establish comprehensive REST protocol semantics or exhaustive malformed-input behavior. See [Domain Gap Assessment](DOMAIN_GAP_ASSESSMENT.md) for the current audit and implementation priorities.

## Upgrade guidance

- For consumers using the core API, reference `TheSingularityWorkshop.FSM_Rest` directly and provide an `IRestTransport` implementation or use the supplied `HttpClientRestTransport`.
- Add `TheSingularityWorkshop.FSM_Rest.OpenApi` only when importing supported OpenAPI JSON definitions.
- Add `TheSingularityWorkshop.FSM_Rest.COS` only when integrating REST capability metadata with FSM_COS/MicroBundleDomain.
- Configure authentication, authorization, timeout, retry, and error-handling policy in the host. Review the consuming guide and re-run integration tests when upgrading between alpha versions.

## Release evidence and publication

This note is descriptive, not release approval. See the [Release Verification Checklist](RELEASE_CHECKLIST.md) for exact-SHA CI and artifact evidence. Publishing to NuGet requires explicit release-owner approval and a separate authorized publish action.
