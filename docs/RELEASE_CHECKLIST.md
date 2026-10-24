# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Latest candidate audit snapshot

- **Candidate commit:** [`94038ddcfc59e2092d7673cee5569a8d6b0616a5`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/94038ddcfc59e2092d7673cee5569a8d6b0616a5) on `development`.
- **CI run:** [Package FSM REST #40069735788](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40069735788) — build-and-test and source-integration passed on this exact SHA; publication was skipped.
- **Package artifact:** workflow artifact `12550409529`, `fsm-rest-nuget-package`, ZIP SHA-256 `583bd9e8d18f1a4df0daa618b9d614b44c6a9599f2e44f2fd031fde571c48a47`.
- **Inspected packages:** `TheSingularityWorkshop.FSM_Rest` `0.1.0-alpha.5` (depends on `TheSingularityWorkshop.FSM_API` `1.0.13`); `TheSingularityWorkshop.FSM_Rest.OpenApi` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5`); `TheSingularityWorkshop.FSM_Rest.COS` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`). All three contain a `net8.0` assembly, XML API documentation, README, and license file. Core has no MicroBundleDomain or FSM_COS dependency.
- **Artifact inspection:** the archive contains exactly these three `.nupkg` files. Their nuspec dependency identities and versions match the intended package boundaries above. The packages contain XML docs, README, and license files. Repository metadata was not present in the inspected nuspecs and remains a consumer-discoverability improvement to evaluate; it is not being represented as verified.
- **Scope of evidence:** this is the inspected package snapshot for the exact candidate SHA. Any subsequent source or documentation commit requires a new exact-SHA CI run; publication remains a separate, explicitly approved action.

## Package boundary

- [ ] Core package restores and builds without an FSM_COS dependency or unintended Workshop-package dependency.
- [ ] FSM_COS integration remains in the separate `FSM_REST.COS` project/package; do not move composition adapters into core.
- [ ] Packed NuGet metadata and dependency groups match the intended package boundary and supported target frameworks.
- [ ] The package does not claim to implement authentication, retries, caching, endpoint discovery, OpenAPI/YAML parsing, domain schemas, GUI manifestation, or experience lifecycle unless those capabilities are explicitly implemented and tested. OpenAPI support is provided by `TheSingularityWorkshop.FSM_Rest.OpenApi`, not core.

## Behavior and API

- [x] Release build succeeds.
- [x] Automated test suite passes on the candidate SHA; domain coverage gaps still listed in `DOMAIN_GAP_ASSESSMENT.md` must not be mistaken for comprehensive REST semantics.
- [x] Coverage collection and upload complete; review the report for meaningful gaps rather than treating a percentage alone as proof.
- [ ] Public XML documentation is generated and reviewed; resolve unexplained warnings and misleading API descriptions.
- [ ] README examples can be followed against the current public API and do not depend on unpublished or stale APIs.
- [ ] Documentation explains the boundary between reusable capability metadata and changing runtime response data.
- [ ] Diagrams match current source and clearly distinguish conceptual illustrations from generated or runtime-verified evidence.

## CI and package inspection

- [x] CI passed on the recorded candidate SHA, including core build/tests and source-integration build/tests. Re-run after any subsequent source change.
- [x] Create the package artifact and inspect its nuspec, dependency groups, included files, license/readme metadata, and version for the recorded candidate.
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
