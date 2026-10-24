# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Release notes

- [x] Added [release notes](RELEASE_NOTES.md) describing the current candidate package set, architecture boundaries, compatibility, known limitations, and upgrade guidance. Review these notes against the final exact candidate before publication.

## Latest candidate artifact inspection

- **Verified development commit:** [`6934a2a80a6b5951f74cf8195b39573cdafdc278`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/6934a2a80a6b5951f74cf8195b39573cdafdc278).
- **CI run:** [Package FSM REST #40119874076](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40119874076) — build-and-test and source-integration passed; publish job was skipped as intended.
- **Exact package artifact inspected:** artifact `12570755744`, `fsm-rest-nuget-package`, ZIP SHA-256 `25b82da0fd1dd9ca8d75b79025b1d6ed963f3fc4684be6159a38da8612667182`. The archive contains exactly three packages: core `TheSingularityWorkshop.FSM_Rest.0.1.0-alpha.5.nupkg`, `TheSingularityWorkshop.FSM_Rest.OpenApi.0.1.0-alpha.1.nupkg`, and `TheSingularityWorkshop.FSM_Rest.COS.0.1.0-alpha.1.nupkg`.
- **Manual inspection findings:** all three nuspecs identify repository URL `https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST` and commit `6934a2a80a6b5951f74cf8195b39573cdafdc278`; each has `net8.0` dependency metadata, XML documentation, README and license entries. Core depends on `TheSingularityWorkshop.FSM_API` `1.0.13` and does not declare MicroBundleDomain. OpenApi depends on core `0.1.0-alpha.5`. COS depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`.
- **Per-package SHA-256:** core `f5be72571d6a1244a86614c9260617b200ec2e795db8513827e7a69ac4649f4d`; OpenApi `07ae665fbfc8d50e10916e95b0d1babb1fb3ec6154211b1dce59b25ea12e2134`; COS `e32b772cea94e543f426b51f7ae5dd441a6b9bbf2d2233cdc01d1b16a5cf6e3c`.
- **Packaged README-link audit:** core (31 links), OpenApi (4), and COS (1) were inspected; zero unresolved local paths and no stale repository-relative `../../` paths remained in the new artifact.
- **Evidence boundary:** this inspection applies only to the exact artifact and commit above. This checklist update is a subsequent documentation commit; a fresh successful exact-SHA run and matching artifact are still required for the final candidate. Publication remains a separate, explicitly approved action.
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

- [x] CI passed on the previously inspected commit `6934a2a80a6b5951f74cf8195b39573cdafdc278` (run #40119874076), including build-and-test and source-integration. This checklist update creates a new candidate commit and requires its own exact-SHA run and artifact inspection before release evidence is current.
- [x] Manually download and inspect the exact candidate artifact: package identities/versions, nuspec dependencies, framework assets, XML docs, README/license, repository metadata, and packaged README links (artifact `12570755744`, SHA-256 recorded above).
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
