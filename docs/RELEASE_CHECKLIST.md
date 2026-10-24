# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Latest candidate artifact inspection

- **Verified development commit:** [`cd716549d68926168aa5a00dad9dd68f9df3e286`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/cd716549d68926168aa5a00dad9dd68f9df3e286).
- **CI run:** [Package FSM REST #40118238040](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40118238040) — build-and-test and source-integration passed; publish job was skipped as intended.
- **Exact package artifact inspected:** artifact `12570032974`, `fsm-rest-nuget-package`, ZIP SHA-256 `bb6fb99905573449b7511a17a995242d3caaff5c4806d6a8a189805a3544759e`. The archive contains exactly three packages: core `TheSingularityWorkshop.FSM_Rest.0.1.0-alpha.5.nupkg`, `TheSingularityWorkshop.FSM_Rest.OpenApi.0.1.0-alpha.1.nupkg`, and `TheSingularityWorkshop.FSM_Rest.COS.0.1.0-alpha.1.nupkg`.
- **Manual inspection findings:** all three nuspecs identify repository URL `https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST` and commit `cd716549d68926168aa5a00dad9dd68f9df3e286`; each has `net8.0` dependency metadata, XML documentation, README and license entries. Core depends on `TheSingularityWorkshop.FSM_API` `1.0.13` and does not declare MicroBundleDomain. OpenApi depends on core `0.1.0-alpha.5`. COS depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`.
- **Per-package SHA-256:** core `b5a91993a2cb6dcbc98254cf7948ac6d74a6b9025a14605a572a1e0e30b961b6`; OpenApi `63157b7d04c1332178705b20920c9402b547771882966f451dab37def3c31c91`; COS `8821e940d5bb5d7b6d020731d5cc71147f33a0705fb886c361943d3820a85f23`.
- **Packaged README-link audit:** core (29 links), OpenApi (4), and COS (1) were inspected; zero unresolved local paths and no stale repository-relative `../../` paths remained in the new artifact.
- **Evidence boundary:** this inspection applies only to the exact artifact and commit above. Any subsequent source or documentation commit requires a fresh successful exact-SHA run and a matching artifact. Publication remains a separate, explicitly approved action.
## Package boundary

- [ ] Core package restores and builds without an FSM_COS dependency or unintended Workshop-package dependency.
- [ ] FSM_COS integration remains in the separate `FSM_REST.COS` project/package; do not move composition adapters into core.
- [x] Packed NuGet metadata and dependency groups match the intended package boundary and the current `net8.0` target.
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

- [x] CI passed on commit `cd716549d68926168aa5a00dad9dd68f9df3e286` (run #40118238040), including build-and-test and source-integration. This checklist-only update itself will require a fresh run before its content is treated as current release evidence.
- [x] Manually download and inspect the exact candidate artifact: package identities/versions, nuspec dependencies, framework assets, XML docs, README/license, repository metadata, and packaged README links (artifact `12570032974`, SHA-256 recorded above).
- [x] Verify first-party package dependencies and intended core/optional-adapter boundaries against the current candidate artifact. Broader ecosystem target-framework compatibility remains a separate release consideration.
- [x] Confirm repository documentation links remain useful from both the repository and the packed package. The exact current artifact was re-inspected after the documentation-link repair; packaged README links had zero unresolved local paths.
- [ ] Review the release notes for accurate scope, compatibility, known limitations, and upgrade guidance.

## Publication gate

- [ ] Record the exact commit SHA, package version, CI run, and inspected artifact for the final candidate.
- [ ] Confirm the release owner has explicitly approved publication.
- [ ] Keep publishing disabled/skipped until that explicit approval is given.
- [ ] After an authorized publication, verify the published package metadata and download/install path.

## Architectural invariant

A REST capability description explains how an operation may be invoked; the request and transport execute it, and the returned response is runtime data. Keep those concerns distinct, and keep downstream composition outside the core package.

This checklist records release criteria only. Completing it does not itself authorize publication.
