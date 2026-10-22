# FSM_REST Branch Reconciliation Ledger

**Snapshot date:** 2026-10-22 (refreshed after CI run `39857589458`)  
**Comparison base:** `development`  
**Purpose:** Preserve unique work before any surplus branch is manually removed. This ledger is a review aid, not authorization to delete branches.

> **Sith policy:** keep `master` stable and `development` as the active work branch. The repository owner performs branch deletion manually only after reconciliation. No branch has been deleted as part of this audit.

## Branch comparison snapshot

| Branch | Ahead of development | Behind development | Status | Required disposition |
|---|---:|---:|---|---|
| `architecture/decouple-rest-microbundle-from-core` | 0 | 44 | Behind | Branch tip is the merge base and there are no branch-only commits. Candidate for owner deletion after final history check. |
| `forge/fsm-rest-documentation-and-capability-flow` | 16 | 75 | Diverged | Reconcile before deletion; review documentation, diagrams, request factory, and test changes. |
| `forge/fsm-rest-functional-standard` | 14 | 76 | Diverged | Reconcile before deletion; transport/core API/test paths overlap current development; retain any unique branch history until fully reviewed. |
| `release-prep/fsm-rest-1.0.0` | 13 | 60 | Diverged | Reconcile before deletion; historical release docs and a visual require review; do not accept stale release claims unchanged. |

Counts are from GitHub's compare endpoint with `development` as the base. The changed-file lists below describe branch diffs from Git's merge base; a file appearing in a list does **not** prove the branch's version is absent from development.

## Candidate work to inspect and preserve

### `forge/fsm-rest-documentation-and-capability-flow`

The branch diff reports changes in:

- `README.md`, `docs/MICROBUNDLE.md`, `docs/REFLECTION.md`, `docs/THEORY.md`
- `docs/assets/fsm-rest-capability-recipe.svg`
- `docs/assets/fsm-rest-composition-map.svg`
- `docs/assets/fsm-rest-execution-flow.svg`
- `src/FSM_REST/RestRequestFactory.cs`
- `Program.cs` and `tests/FSM_REST.Tests/RestDescriptorTests.cs`

The current `development` branch already contains the named documentation, SVG assets, and request-factory path. The branch's `RestRequestFactory` implementation is already present on `development`; the current test file also contains its path/query/header/cookie binding and rejection tests, plus additional tests rejecting base URIs containing a query or fragment. The branch version therefore does not currently justify overwriting the active implementation. Its README and MicroBundle/theory text differ substantially in size and wording, so keep the current richer docs as the baseline and continue reviewing branch-only commits for any precise explanation that is genuinely missing. The three named SVGs and request-factory path are already present.

### `forge/fsm-rest-functional-standard`

The branch diff reports changes in:

- `.github/workflows/build.yml`, `README.md`, and `src/FSM_REST/FSM_REST.csproj`
- Core descriptor and transport files under `src/FSM_REST/`
- `tests/FSM_REST.Tests/RestDescriptorTests.cs` and `RestTransportTests.cs`

These paths exist in the current repository, but the branch may contain distinct historical implementations or test coverage. A fresh comparison against the current development snapshot reports 14 commits ahead and 75 behind. The branch's transport and descriptor paths overlap the current source, so compare behavior rather than line counts; do not overwrite the active implementation with an older branch snapshot. A direct content comparison of `tests/FSM_REST.Tests/RestTransportTests.cs` shows its full content is identical on the branch and current `development` (same blob SHA). The five transport cases—request/response capture, non-success status preservation, cancellation, null client, and null request—are already present, so no test code needs porting from this file.

### `release-prep/fsm-rest-1.0.0`

The branch diff reports:

- `docs/RELEASE_1_0.md` — **not present on current development** at this snapshot.
- `docs/assets/release-frontier.svg` — **not present on current development** at this snapshot.
- Changes to the build workflow, README, MicroBundle documentation, project file, and descriptor-binding tests.
- Removal of `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs` from the core-project path.

The current architecture keeps the FSM_COS integration in the separate `src/FSM_REST.COS/` project, including its own `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs`. Do not restore those integration types into the core merely because the release branch had a different layout. A fresh comparison against the current development snapshot reports 13 commits ahead and 60 behind. Review `RELEASE_1_0.md` and `release-frontier.svg` for durable release criteria or useful visual explanations; harvest relevant material into current documentation if it remains accurate, then leave deletion to the owner.

### Release checklist harvested into current development

The durable verification criteria from the historical `docs/RELEASE_1_0.md` have been refreshed in [`docs/RELEASE_CHECKLIST.md`](RELEASE_CHECKLIST.md). The checklist now requires checking the actual candidate commit, current package metadata/dependencies, core-versus-`FSM_REST.COS` boundary, behavior and coverage, packed NuGet contents, documentation/diagram provenance, and explicit owner approval before publication. It intentionally does not repeat the old `1.0.0` version or stable-FSM_COS prerequisite as fact. The historical SVG was not copied into current docs because its labels make those stale claims; its useful capability-versus-runtime-data idea is retained in the checklist and current theory documentation.

### Release contract and frontier visual: reviewed

The unique `docs/RELEASE_1_0.md` provides a useful release-checklist skeleton: independent restore/build/test/coverage, XML documentation review, package dependency inspection, practical README examples, theory and visual boundary checks, and explicit publication gating. Those criteria should be retained as a checklist, but the branch document is **not** accepted as a current 1.0 release contract without revision: the current work is focused on the existing development line, and its release status/prerequisites must be revalidated against current package metadata and FSM_COS Alpha.7.

The unique `docs/assets/release-frontier.svg` illustrates capability recipe → operation descriptor → runtime binding → request/transport → current remote data, which is a helpful explanation of the capability-versus-response boundary. However, its labels assert `FSM_REST 1.0.0` and `FSM_COS 1.0.0 stable prerequisite`; those are historical branch claims, not verified current release facts. Do not promote the SVG as current release evidence unchanged. If reused, revise the labels and mark it conceptual or source-checked according to the actual generation process.

This review preserves the substantive value in the ledger without copying stale release claims into the active documentation.

### Follow-up after direct file comparison

The current `development` implementation of `RestRequestFactory` and `RestDescriptorTests.cs` already includes the request-binding feature and the branch's core edge cases; development adds explicit base-URI query/fragment rejection coverage. A fresh source-of-truth check also confirmed `RestMicroBundle` and `RestApiDescriptorExtensions` exist only under `src/FSM_REST.COS/`, not in the core project. The wording in `docs/MICROBUNDLE.md` was corrected to make this optional package boundary explicit and to show the required namespace. No code was copied from this branch because this audit found no missing implementation to port. The functional-standard branch's transport tests have been directly compared and are identical to current `development`; no transport tests need porting. The ledger commit `cf632222d63491075d18d7e52a3a9b14c9fd0db1` passed the complete `Package FSM REST` workflow: build/test/coverage, NuGet package creation, and source-integration build/tests. The two latest documentation commits also passed: checklist commit `17f321e8acc1b65251ba407fbb9928fafb2ab052` (run `39855235239`) and ledger commit `34f2d0439d3367c4d3a6bccd8e65b713dfc47fec` (run `39855248904`), each with build/test and source-integration successful and `publish_nuget` skipped. This remains a targeted file/content comparison, not yet a complete commit-by-commit audit.

## Manual completion checklist

- [ ] Inspect branch-only commits, not just the current changed-file summary. Compare metadata refreshed: architecture 0 ahead/44 behind; capability/docs 16/75; functional-standard 14/76; release-prep 13/60.
- [ ] Compare candidate docs and SVGs against current `development`; retain accurate missing explanations.
- [ ] Compare test intent and edge cases; port missing tests into the current test project.
- [ ] Confirm no useful release criteria or package-boundary decisions remain only in branch history.
- [x] Re-run CI after the latest checklist and ledger updates; runs `39855235239` and `39855248904` passed build/test and source-integration. Publishing was skipped.
- [ ] Re-run CI after any future harvested source or documentation changes. Latest ledger-only run `39855573430` passed build/test and source-integration; `publish_nuget` was skipped. The MicroBundle boundary correction and example wording are now verified by run `39857589458`: build/test and source-integration passed; `publish_nuget` was skipped.
- [ ] Owner manually deletes only branches whose unique work has been reconciled.

**No publication or release action is authorized by this ledger.**
