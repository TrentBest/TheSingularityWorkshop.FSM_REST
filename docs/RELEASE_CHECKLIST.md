# FSM_REST Release Verification Checklist

Use this checklist for a candidate package release. It is a verification gate, not a declaration that a particular version is ready. Confirm the current package version, target frameworks, dependencies, and upstream compatibility from the checked-out source and CI; do not inherit version or prerequisite claims from historical branch documents. The artifact evidence below is retained for traceability only: later source and test changes mean it is not evidence for the current `development` head. A fresh exact-SHA CI run and package artifact inspection are required before release.

## Release notes

- [x] Added [release notes](RELEASE_NOTES.md) describing the current candidate package set, architecture boundaries, compatibility, known limitations, and upgrade guidance. Review these notes against the final exact candidate before publication.

## Historical candidate artifact inspection — superseded by subsequent development changes

- **Last inspected implementation candidate:** [`cb03e7e049da391d11cc7accdc9a8acf0e609610`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/cb03e7e049da391d11cc7accdc9a8acf0e609610) on `development`.
- **Exact-SHA CI run:** [Package FSM REST #40164358155](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40164358155) — build-and-test and source-integration passed; NuGet publication was skipped.
- **Test result:** 71 passed, 0 failed.
- **Package archive:** artifact `12587164237`, `fsm-rest-nuget-package.zip`; SHA-256 `cabe7184ad278d9bd765bfa803abd8495f7386f68b4a642140a71925320dc7d8`.
- **Core package:** `TheSingularityWorkshop.FSM_Rest.0.1.0-alpha.5.nupkg`; SHA-256 `ffa0c85a9193873238dab239a2ac6197605a8071f870aa27774b82bfb86f20e2`.
- **OpenAPI package:** `TheSingularityWorkshop.FSM_Rest.OpenApi.0.1.0-alpha.1.nupkg`; SHA-256 `4b9cee5b30d8df344f5ad4f585a4f5d6fcc39d2755150f858d72ddd79279156d`.
- **FSM_COS adapter package:** `TheSingularityWorkshop.FSM_Rest.COS.0.1.0-alpha.1.nupkg`; SHA-256 `8637b86778ccc439bbc6425572df5aae70f986358ee998ca3656903c99ce33d0`.
- **Manual package inspection:** the three package nuspecs identify the exact commit above; the core package depends on FSM_API `1.0.13` without a MicroBundleDomain dependency, OpenAPI depends on core alpha.5, and COS depends on core alpha.5 plus MicroBundleDomain `1.0.1`. All three packaged READMEs were checked and no unresolved local links were found.
- **Evidence boundary:** this inspection applies only to the exact implementation SHA and artifacts listed here. Subsequent documentation changes create a newer candidate and require a fresh successful exact-SHA CI run plus matching artifact inspection before release. Publication remains a separate, explicitly approved action.

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
- [ ] Confirm repository documentation links remain useful from both the repository and the packed package. Manual inspection of the previous artifact found that the core README's relative storefront-sample link was not packaged; the link has been changed to a GitHub URL, and the new package-content link gate must pass on the current candidate.
- [x] CI audits relative Markdown links, Markdown images, and HTML `href`/`src` targets in each packed root README against the actual package contents; the first discovered missing storefront-sample link was changed to a GitHub URL.
- [ ] Review the release notes for accurate scope, compatibility, known limitations, and upgrade guidance.

## Publication gate

- [ ] Record the exact commit SHA, package version, CI run, and inspected artifact for the final candidate.
- [ ] Confirm the release owner has explicitly approved publication.
- [x] CI workflow keeps publishing disabled/skipped by default and requires manual dispatch, `publish=true`, the `development` ref, and success from both `build-and-test` and `source-integration` before the publish job can run.
- [ ] Keep publishing disabled/skipped until the release owner explicitly approves publication.
- [ ] After an authorized publication, verify the published package metadata and download/install path.

## Architectural invariant

A REST capability description explains how an operation may be invoked; the request and transport execute it, and the returned response is runtime data. Keep those concerns distinct, and keep downstream composition outside the core package.

This checklist records release criteria only. Completing it does not itself authorize publication.


## Latest exact-SHA verification — 2026-10-27

- **Candidate source SHA:** [`f134170f93b4eaa2f315f915412e574e6799d697`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/f134170f93b4eaa2f315f915412e574e6799d697) on `development`.
- **CI run:** [Package FSM REST #40445151564](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40445151564) — `build-and-test` and `source-integration` succeeded; `publish_nuget` was skipped.
- **Artifact gate:** all three package artifacts were packed and the automated packed-artifact audit passed, including package identity/version, required content, dependency boundaries, and README relative-link checks. GitHub artifact `fsm-rest-nuget-package` SHA-256: `e7f427e3359fb7c680a64695ff778d12587cc93a78a7ae80e5b38ef9489508c5`.
- **Packaging fix:** four internal documents (`BRANCH_RECONCILIATION.md`, `DOMAIN_GAP_ASSESSMENT.md`, `FSM_REST.DOCUMENTATION_READINESS.md`, and `RELEASE_CHECKLIST.md`) are excluded from core/OpenAPI package documentation, and CI explicitly rejects their presence in generated packages. The public README no longer links to the internal checklist.
- **Versions unchanged:** core `0.1.0-alpha.5`, OpenAPI `0.1.0-alpha.1`, COS `0.1.0-alpha.1`.
- **Release boundary:** this is successful exact-SHA CI and automated artifact-gate evidence, not owner approval or proof that every unchecked manual review is complete. Manual archive/content review, release-note/public-contract review, and remaining behavior gaps are still open. Do not publish without explicit owner approval.

## Latest exact-SHA verification — 2026-10-27 (follow-up run)

- **Candidate source SHA:** [`bb80b8f51e3b53408532fbcb2cc15adc58b4fa8e`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/bb80b8f51e3b53408532fbcb2cc15adc58b4fa8e) on `development`; this commit corrects the recorded verification artifact digest.
- **CI run:** [Package FSM REST #40445533201](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40445533201) — `build-and-test` and `source-integration` succeeded; `publish_nuget` was skipped.
- **Automated package gate:** packing and packed-NuGet artifact audit passed, including package identity/version, required content, dependency boundaries, and README relative-link checks. The source-integration build and tests also passed.
- **Current GitHub Actions package-artifact metadata:** artifact ID `12708385781`, name `fsm-rest-nuget-package`, archive size 170,294 bytes, reported SHA-256 `2bfc887e00729a09661d5f6a2797b5a49855a3f9a6c1e272faf60c93737acdb2`; associated with the exact source SHA above. This is the GitHub Actions artifact archive digest, not a separately verified SHA-256 for each extracted `.nupkg`.
- **Test/coverage artifact metadata:** artifact ID `12708680336`, name `fsm-rest-test-results`, reported SHA-256 `f8af201b4fa5efddcb3876f844b6388123ca185d3d1b707c0e84395a095483b8`; associated with the same source SHA.
- **Versions remain:** core `0.1.0-alpha.5`, OpenAPI `0.1.0-alpha.1`, COS `0.1.0-alpha.1`.
- **Evidence boundary:** this records the newer exact-SHA CI run and its artifact metadata. It does not replace manual archive/content inspection of the current downloaded package, complete the unchecked public-contract/release-note review, or establish comprehensive REST semantics. Publication remains explicitly approval-gated.

