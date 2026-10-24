# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Package boundary

- [ ] Core package restores and builds without an FSM_COS dependency or unintended Workshop-package dependency.
- [ ] FSM_COS integration remains in the separate `FSM_REST.COS` project/package; do not move composition adapters into core.
- [ ] Packed NuGet metadata and dependency groups match the intended package boundary and supported target frameworks.
- [ ] The package does not claim to implement authentication, retries, caching, endpoint discovery, OpenAPI/YAML parsing, domain schemas, GUI manifestation, or experience lifecycle unless those capabilities are explicitly implemented and tested. OpenAPI support is provided by `TheSingularityWorkshop.FSM_Rest.OpenApi`, not core.

## Behavior and API

- [ ] Release build succeeds with no unexplained warnings.
- [ ] Complete automated test suite passes, including request construction, parameter binding, transport behavior, cancellation, and failure/invalid-input cases.
- [ ] Coverage collection completes; review the report for meaningful gaps rather than treating a percentage alone as proof.
- [ ] Public XML documentation is generated and reviewed; resolve unexplained warnings and misleading API descriptions.
- [ ] README examples can be followed against the current public API and do not depend on unpublished or stale APIs.
- [ ] Documentation explains the boundary between reusable capability metadata and changing runtime response data.
- [ ] Diagrams match current source and clearly distinguish conceptual illustrations from generated or runtime-verified evidence.

## CI and package inspection

- [ ] CI passes on the exact commit intended for release, including core build/tests and source-integration build/tests.
- [ ] Create the package artifact and inspect its nuspec, dependency groups, included files, license/readme metadata, and version.
- [ ] Verify any first-party dependencies against the actual release plan; do not assume a dependency is stable or compatible based on a historical diagram.
- [ ] Confirm repository documentation links remain useful from both the repository and the packed package.
- [ ] Review the release notes for accurate scope, compatibility, known limitations, and upgrade guidance.

## Publication gate

- [ ] Record the exact commit SHA, package version, CI run, and inspected artifact.
- [ ] Confirm the release owner has explicitly approved publication.
- [ ] Keep publishing disabled/skipped until that explicit approval is given.
- [ ] After an authorized publication, verify the published package metadata and download/install path.

## Architectural invariant

A REST capability description explains how an operation may be invoked; the request and transport execute it, and the returned response is runtime data. Keep those concerns distinct, and keep downstream composition outside the core package.

This checklist records release criteria only. Completing it does not itself authorize publication.
