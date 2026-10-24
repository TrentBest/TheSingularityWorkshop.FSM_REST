# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Latest candidate audit snapshot

- **Candidate commit:** [`031562b080017b85b78cabed153cb8b7bdc2147c`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/031562b080017b85b78cabed153cb8b7bdc2147c) on `development`.
- **CI run:** [Package FSM REST #40063906219](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40063906219) — build/test, source integration, deterministic demo, coverage upload, package audit, and artifact upload passed; publication was skipped.
- **Latest checklist-commit validation:** [CI run #40065950126](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40065950126) passed on `c1827a8a23a90f61ac274e833112436b1c658cee` (documentation-only checklist update); build/test and source integration passed, package audit/artifact upload passed, and publication was skipped. This confirms the current branch commit is green but does not replace the separately identified package artifact snapshot below.
- **Package artifact:** workflow artifact `12548278893`, `fsm-rest-nuget-package`, ZIP SHA-256 `75b3498b3a0e1c8ebd8ef07e6e0d990e3d4c16cdee2f0ea285df44496cf68801`.
- **Inspected packages:** `TheSingularityWorkshop.FSM_Rest` `0.1.0-alpha.5` (depends on `TheSingularityWorkshop.FSM_API` `1.0.13`); `TheSingularityWorkshop.FSM_Rest.OpenApi` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5`); `TheSingularityWorkshop.FSM_Rest.COS` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`). All three contain a `net8.0` assembly, XML API documentation, README, and license file. Core has no MicroBundleDomain or FSM_COS dependency.
- **Scope of evidence:** this snapshot documents the inspected artifacts from that run. Any source change requires a new exact-SHA CI run; publication remains a separate, explicitly approved action.

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
