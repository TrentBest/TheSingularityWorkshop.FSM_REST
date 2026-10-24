# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Latest candidate artifact inspection (before this checklist update)

- **Verified development commit:** [`cd659d4002d34d2a4073f74b10d42219ff3bd6f7`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/cd659d4002d34d2a4073f74b10d42219ff3bd6f7).
- **CI run:** [Package FSM REST #40070991652](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40070991652) — build-and-test and source-integration passed; the automated package audit passed; the publish job was skipped.
- **Exact package artifact inspected:** artifact `12550918571`, `fsm-rest-nuget-package`, ZIP SHA-256 `3cbccd347af6da462c7519ddeb2483fbd1f57b9b0f37819a75cec74afd36100f`. The archive contains exactly three packages: core `TheSingularityWorkshop.FSM_Rest.0.1.0-alpha.5.nupkg`, `TheSingularityWorkshop.FSM_Rest.OpenApi.0.1.0-alpha.1.nupkg`, and `TheSingularityWorkshop.FSM_Rest.COS.0.1.0-alpha.1.nupkg`.
- **Manual inspection findings:** all three nuspecs identify repository URL `https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST` and commit `cd659d4002d34d2a4073f74b10d42219ff3bd6f7`; each has `net8.0` dependency metadata, XML documentation, README and license entries. Core depends on `TheSingularityWorkshop.FSM_API` `1.0.13` and does not declare MicroBundleDomain. OpenApi depends on core `0.1.0-alpha.5`. COS depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`. The package versions and dependency boundaries match the current project files.
- **Per-package SHA-256:** core `ac95a0a156bbbfd04405eea72094291a4dd5bdc829a1bd463adf48819bb503ae`; OpenApi `44cc2d3117d6901224f5720e9fc198e065557415cc202f9b6fc04989cda9982d`; COS `2e8eb2cf8309011467b9894063b14ba5218e78bb263bdeda6ca6a4986cb39527`.
- **Evidence boundary:** this inspection is for the exact artifact and commit listed above. Any subsequent source or documentation commit requires a fresh successful exact-SHA run and a matching artifact. Publication remains a separate, explicitly approved action.

## Package boundary

- [ ] Core package restores and builds without an FSM_COS dependency or unintended Workshop-package dependency.
- [ ] FSM_COS integration remains in the separate `FSM_REST.COS` project/package; do not move composition adapters into core.
- [ ] Packed NuGet metadata and dependency groups match the intended package boundary and supported target frameworks.
- [ ] The package does not claim to implement authentication, retries, caching, endpoint discovery, OpenAPI/YAML parsing, domain schemas, GUI manifestation, or experience lifecycle unless those capabilities are explicitly implemented and tested. OpenAPI support is provided by `TheSingularityWorkshop.FSM_Rest.OpenApi`, not core.

## Behavior and API

- [x] Release build succeeds.
- [x] Automated test suite passes on the latest recorded CI SHA; domain coverage gaps listed in `DOMAIN_GAP_ASSESSMENT.md` must not be mistaken for comprehensive REST semantics.
- [x] Coverage collection and upload complete; review the report for meaningful gaps rather than treating a percentage alone as proof.
- [ ] Public XML documentation is generated and reviewed; resolve unexplained warnings and misleading API descriptions.
- [ ] README examples can be followed against the current public API and do not depend on unpublished or stale APIs.
- [ ] Documentation explains the boundary between reusable capability metadata and changing runtime response data.
- [ ] Diagrams match current source and clearly distinguish conceptual illustrations from generated or runtime-verified evidence.

## CI and package inspection

- [x] CI passed on the latest recorded development SHA, including core build/tests, source-integration build/tests, deterministic demo, and automated package audit. Re-run after this checklist edit and any subsequent source change.
- [x] Manually download and inspect the exact artifact selected for release: package identities/versions, nuspec dependencies, framework assets, XML docs, README/license, and repository metadata (artifact `12550918571`, SHA-256 recorded above).
- [ ] Verify first-party dependencies against the actual release plan; do not assume a dependency is stable or compatible based on a historical diagram.
- [ ] Confirm repository documentation links remain useful from both the repository and the packed package.
- [ ] Review the release notes for accurate scope, compatibility, known limitations, and upgrade guidance.

## Publication gate

- [ ] Record the exact commit SHA, package version, CI run, and inspected artifact for the final candidate.
- [ ] Confirm the release owner has explicitly approved publication.
- [ ] Keep publishing disabled/skipped until that explicit approval is given.
- [ ] After an authorized publication, verify the published package metadata and download/install path.

## Architectural invariant

A REST capability description explains how an operation may be invoked; the request and transport execute it, and the returned response is runtime data. Keep those concerns distinct, and keep downstream composition outside the core package.

This checklist records release criteria only. Completing it does not itself authorize publication.
