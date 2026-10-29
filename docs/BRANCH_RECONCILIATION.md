<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Branch Reconciliation Ledger

**Snapshot date:** 2026-10-24  
**Comparison base:** `development` at `0c2f78209f0e61152951ce46eb2d42dc0785f2dc`  
**CI evidence:** [Package FSM REST run 40060933128](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40060933128) passed on that exact SHA; NuGet publication was not performed.  
**Purpose:** Preserve unique work before any surplus branch is manually removed. This ledger is a review aid, not authorization to delete branches.

> **Sith policy:** keep `master` stable and `development` as the active work branch. The repository owner performs branch deletion manually only after reconciliation. No branch has been deleted as part of this audit.

## Current branch inventory

The GitHub branch endpoint currently reports six branches: `master`, `development`, and the four review branches below. The prior snapshot is stale; use these fresh comparisons for the next manual review.

| Branch | Ahead of development | Behind development | Status | Required disposition |
|---|---:|---:|---|---|
| `architecture/decouple-rest-microbundle-from-core` | 0 | 277 | Behind | No branch-only commits at this comparison. Owner may consider deletion after final history check. |
| `forge/fsm-rest-documentation-and-capability-flow` | 16 | 308 | Diverged | Inspect branch-only commits and preserve any genuinely unique material before deletion. |
| `forge/fsm-rest-functional-standard` | 14 | 309 | Diverged | Compare behavior/tests against current development; do not overwrite active source with historical snapshots. |
| `release-prep/fsm-rest-1.0.0` | 13 | 293 | Diverged | Review unique release documentation and SVG; do not adopt stale 1.0.0 or stable-prerequisite claims. |

Counts come from GitHub's compare endpoint with `development` as the base. A changed-file list does not prove that a branch's version is missing from development. These are history comparisons, not merge recommendations.

## Candidate work to inspect and preserve

### `forge/fsm-rest-documentation-and-capability-flow`

Reported changed paths include `README.md`, `docs/MICROBUNDLE.md`, `docs/REFLECTION.md`, `docs/THEORY.md`, three capability/composition/execution SVGs, `src/FSM_REST/RestRequestFactory.cs`, `Program.cs`, and `tests/FSM_REST.Tests/RestDescriptorTests.cs`.

The current development tree already contains the named documentation, SVG assets, and request-factory path. The active request factory and tests include path/query/header/cookie binding and rejection cases, including rejection of base URIs containing query or fragment components. Do not replace current richer documentation or implementation with an older branch snapshot. Review branch-only commits for precise explanations or tests still absent from development.

### `forge/fsm-rest-functional-standard`

Reported changed paths include the build workflow, README, core project metadata, core descriptor/transport files, and descriptor/transport tests. These paths overlap the current implementation. The previous direct file comparison found `tests/FSM_REST.Tests/RestTransportTests.cs` identical to development at that time, including request/response capture, non-success status preservation, cancellation, and null-client/request cases. Recheck any claimed unique behavior against the current head before porting; do not overwrite active files wholesale.

### `release-prep/fsm-rest-1.0.0`

Unique candidates reported by the previous review:
- `docs/RELEASE_1_0.md` — release checklist skeleton.
- `docs/assets/release-frontier.svg` — capability recipe → operation descriptor → runtime binding → request/transport → changing remote data.

The durable checklist criteria have already been harvested into `docs/RELEASE_CHECKLIST.md`: restore/build/test/coverage, XML docs, package dependencies and contents, practical README examples, architecture boundaries, exact-SHA CI, and explicit owner approval before publication. The old SVG asserts `FSM_REST 1.0.0` and a stable `FSM_COS 1.0.0` prerequisite; those are not verified current facts. If its visual is reused, revise those labels and identify whether it is conceptual or source-checked.

The historical branch also places `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs` under the core-project path. Current architecture keeps these adapters in `src/FSM_REST.COS/`; do not restore them to core based on the historical branch.

### `architecture/decouple-rest-microbundle-from-core`

GitHub reports zero commits ahead of development and 277 behind. There are no branch-only commits according to the current compare endpoint, so this branch has no identified unique commit work to harvest. Owner should perform a final history check before manually deleting it.

## Release boundary reminder

The current development projects configure:
- Core `TheSingularityWorkshop.FSM_Rest`: `0.1.0-alpha.5`
- Optional `TheSingularityWorkshop.FSM_Rest.OpenApi`: `0.1.0-alpha.1`
- Optional `TheSingularityWorkshop.FSM_Rest.COS`: `0.1.0-alpha.1`

These are source-configured candidate versions, not proof of their current NuGet.org publication status. The latest successful workflow is build/test/package-artifact evidence only; its publish job was skipped. Verify NuGet version availability and inspect the exact release artifacts before proposing a publication run. No publication, tag, merge, force-push, or branch deletion is authorized by this ledger.

## Manual completion checklist

- [ ] Inspect branch-only commits, not only changed-file summaries, for the three diverged branches.
- [ ] Confirm the request-factory behavior and test coverage against current development before deciding whether any test intent is missing.
- [ ] Confirm no useful release criteria or accurate visual explanation remain only in the historical release branch.
- [ ] Re-run the full workflow after any source or documentation change and record exact-SHA evidence.
- [ ] Verify candidate versions on NuGet.org and inspect packed artifacts before release approval.
- [ ] Owner manually deletes only branches whose unique work has been reconciled.

**No publication or release action is authorized by this ledger.**

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
