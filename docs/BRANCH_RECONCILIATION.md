<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Branch Reconciliation Ledger

**Snapshot date:** 2026-11-03  
**Comparison base:** `development` at `83cb875df5d60c956b39e5e1d03613f628f190c6`  
**Purpose:** Preserve unique work before any surplus branch is manually removed. This ledger is a review aid, not authorization to delete branches.

> **Sith policy:** keep only `master` and `development` when reconciliation is complete. `development` is the active work branch. The repository owner performs branch deletion manually only after reconciliation. No branch is deleted by this ledger or audit.

## Live branch inventory

GitHub reports **seven live branches** at this snapshot: `master`, `development`, and five review/work branches. Counts below use GitHub compare with `development` as the base at the SHA above.

| Branch | Ahead | Behind | Status | Reconciliation note |
|---|---:|---:|---|---|
| `master` | — | — | Stable | Preserve as stable branch. |
| `development` | 0 | 0 | Active | Current comparison base. |
| `docs/rest-textbook-theory` | 38 | 1 | Diverged | Open draft PR #17; latest reviewed head `5b022c58333404e52fe99e5e1d03613f628f190c6` (see PR for exact current head and CI). |
| `work/httpclient-builder` | 59 | 1 | Diverged | Open draft PR #14; latest reviewed head `cf80fa1ca2d1b0223260f14d55f6cbd517e1670c` (see PR for exact current head and CI). |
| `forge/fsm-rest-documentation-and-capability-flow` | 16 | 468 | Diverged | Inspect branch-only commits; current paths overlap newer development work. |
| `forge/fsm-rest-functional-standard` | 14 | 469 | Diverged | Inspect branch-only commits and compare behavior/tests; do not restore historical snapshots wholesale. |
| `release-prep/fsm-rest-1.0.0` | 13 | 453 | Diverged | Preserve only accurate unique release criteria/assets; reject stale version/prerequisite claims. |

The previously listed `architecture/decouple-rest-microbundle-from-core` branch is **not present** in the live branch inventory. Do not treat it as a current branch or count it among the seven.

Counts are commit-history comparisons, not merge recommendations. A changed-file list does not prove that a branch's version is missing from development. Large behind counts indicate substantial divergence and make branch-only commit inspection essential.

## Open pull-request status

### PR #17 — REST textbook and documentation

- Branch: `docs/rest-textbook-theory`
- Reviewed head: `5b022c58333404e52fe99e750dcb39b4ecf7b642`
- Compare: 38 ahead / 1 behind.
- Exact-head CI run: [40980025868](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40980025868) — build/test and source integration passed; `publish_nuget` skipped.
- Draft and unmerged. No merge or publication is authorized.

The animated moniker URL was verified against the shared documentation standard:
`https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg`

The canonical footer PNG URL was also verified:
`https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png`

The previous version of this ledger incorrectly said six branches, included the absent architecture branch, and used an obsolete comparison base. This snapshot corrects those facts. Do not change the two verified branding URLs.

### PR #14 — HTTP client transport builder

- Branch: `work/httpclient-builder`
- Reviewed head: `cf80fa1ca2d1b0223260f14d55f6cbd517e1670c`
- Compare: 59 ahead / 1 behind.
- Exact-head CI run: [40798363845](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40798363845) — build/test and source integration passed; `publish_nuget` skipped.
- Draft and unmerged. No merge or publication is authorized.

**Architecture review required:** the PR adds `Microsoft.Extensions.Http` to the core project and places `IHttpClientFactory`, `HttpClientRestTransportFactory`, and `IServiceCollection` registration in the core assembly. This couples the framework-neutral core to host DI/factory integration. Keep direct `HttpClient` and `HttpMessageHandler` construction paths in core; move factory and DI integration into a concrete optional adapter project/package (for example, `FSM_REST.HttpClientFactory`) that depends on core. Do not create an empty generic extensions package solely to house this one integration.

## Historical branch work to reconcile

### `forge/fsm-rest-documentation-and-capability-flow`

The current comparison reports 16 commits ahead and 468 behind. Changed paths include README/theory/MicroBundle documentation, three capability/composition/execution SVGs, `RestRequestFactory.cs`, `Program.cs`, and descriptor tests. These paths substantially overlap current development. Inspect the 16 branch-only commits for precise explanations or tests not yet represented; do not replace current richer implementation/docs with older branch snapshots.

### `forge/fsm-rest-functional-standard`

The current comparison reports 14 commits ahead and 469 behind. Changed paths include the build workflow, README, core project metadata, descriptor/transport files, and tests. Earlier inspection found transport tests matching development at that time, including request/response capture, non-success status preservation, cancellation, and null-client/request cases. Recheck branch-only commits against the current tree before porting anything.

### `release-prep/fsm-rest-1.0.0`

The current comparison reports 13 commits ahead and 453 behind. Historical candidates include `docs/RELEASE_1_0.md` and `docs/assets/release-frontier.svg`. Durable release criteria have been harvested into `docs/RELEASE_CHECKLIST.md`: restore/build/test/coverage, XML docs, package dependencies and contents, practical README examples, architecture boundaries, exact-SHA CI, and explicit owner approval before publication.

The historical SVG asserts FSM_REST 1.0.0 and a stable FSM_COS 1.0.0 prerequisite; those are not verified current facts. If its visual is reused, revise those labels and identify whether it is conceptual or source-checked. Historical placement of `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs` under core is also obsolete; current architecture keeps these adapters in `src/FSM_REST.COS/`.

## Package and release boundary

Current source projects and candidate versions must be verified from their project files before release. Source-configured versions are not proof of current NuGet.org publication status. Successful CI/package-artifact generation is not publication evidence. Verify NuGet version availability and inspect exact release artifacts before proposing a publication run.

**No publication, tag, merge, force-push, or branch deletion is authorized by this ledger.**

## Manual completion checklist

- [ ] Inspect branch-only commits—not only changed-file summaries—for all three historical forge/release branches.
- [ ] Confirm request-factory behavior and test coverage against current development before deciding whether any historical test intent is missing.
- [ ] Confirm no useful release criteria or accurate visual explanation remain only in the historical release branch.
- [ ] Resolve PR #14's core-versus-optional HTTP client factory boundary.
- [ ] Re-run the full workflow after any source or documentation change and record exact-SHA evidence.
- [ ] Verify candidate versions on NuGet.org and inspect packed artifacts before release approval.
- [ ] Owner manually deletes only branches whose unique work has been reconciled.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
