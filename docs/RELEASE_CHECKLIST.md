# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents.

## Latest CI verification (before this checklist edit)

- **Verified development commit:** [`138189f08ee428a871dfeb5e48ccc0535fb8615b`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/138189f08ee428a871dfeb5e48ccc0535fb8615b).
- **CI run:** [Package FSM REST #40070046580](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40070046580) — build-and-test and source-integration passed; the automated package audit passed; the publish job was skipped.
- **CI-produced package artifact:** artifact `12550549314`, `fsm-rest-nuget-package`, ZIP SHA-256 `7be3fd815760375be04cd258edd6b575a99c3b33c62e93c74397ad501ab3dec6`. This is evidence of the successful workflow output, **not a claim that this newest ZIP was manually downloaded and inspected**.
- **Package versions expected from the current project files:** core `TheSingularityWorkshop.FSM_Rest` `0.1.0-alpha.5` (depends on `TheSingularityWorkshop.FSM_API` `1.0.13`); `TheSingularityWorkshop.FSM_Rest.OpenApi` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5`); `TheSingularityWorkshop.FSM_Rest.COS` `0.1.0-alpha.1` (depends on core `0.1.0-alpha.5` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`). All target `net8.0`. Confirm the packed nuspecs and assets for the exact release candidate before publication.
- **Previous manually inspected artifact:** candidate commit [`94038ddcfc59e2092d7673cee5569a8d6b0616a5`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/94038ddcfc59e2092d7673cee5569a8d6b0616a5), CI run [#40069735788](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40069735788), artifact `12550409529`, ZIP SHA-256 `583bd9e8d18f1a4df0daa618b9d614b44c6a9599f2e44f2fd031fde571c48a47`. That archive contained the three expected `.nupkg` files; the nuspec dependency identities/versions, `net8.0` assets, XML docs, README/license entries, and repository metadata were inspected for that snapshot only.
- **Evidence boundary:** CI success and the automated package audit do not replace a manual inspection of the exact artifact selected for release. Any subsequent source or documentation commit requires a fresh successful exact-SHA run and a matching artifact. Publication remains a separate, explicitly approved action.

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
- [ ] Manually download and inspect the exact artifact selected for release: package identities/versions, nuspec dependencies, framework assets, XML docs, README/license, and repository metadata.
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
