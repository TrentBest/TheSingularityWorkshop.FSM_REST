# FSM_REST Branch Reconciliation Ledger

**Snapshot date:** 2026-10-22  
**Comparison base:** `development`  
**Purpose:** Preserve unique work before any surplus branch is manually removed. This ledger is a review aid, not authorization to delete branches.

> **Sith policy:** keep `master` stable and `development` as the active work branch. The repository owner performs branch deletion manually only after reconciliation. No branch has been deleted as part of this audit.

## Branch comparison snapshot

| Branch | Ahead of development | Behind development | Status | Required disposition |
|---|---:|---:|---|---|
| `architecture/decouple-rest-microbundle-from-core` | 0 | 34 | Behind | No branch-only commits according to the comparison. Review commit history if desired, then owner may manually delete. |
| `forge/fsm-rest-documentation-and-capability-flow` | 16 | 65 | Diverged | Reconcile before deletion; review documentation, diagrams, request factory, and test changes. |
| `forge/fsm-rest-functional-standard` | 14 | 66 | Diverged | Reconcile before deletion; compare transport/core API/test changes against current development. |
| `release-prep/fsm-rest-1.0.0` | 13 | 50 | Diverged | Reconcile before deletion; it contains release-prep documentation and a visual absent from current development at this snapshot. |

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

The current `development` branch already contains the named documentation, SVG assets, and request-factory path. Before deleting the branch, compare the branch-specific commits and current file contents to make sure no stronger explanation, test case, or implementation detail exists only in its history.

### `forge/fsm-rest-functional-standard`

The branch diff reports changes in:

- `.github/workflows/build.yml`, `README.md`, and `src/FSM_REST/FSM_REST.csproj`
- Core descriptor and transport files under `src/FSM_REST/`
- `tests/FSM_REST.Tests/RestDescriptorTests.cs` and `RestTransportTests.cs`

These paths exist in the current repository, but the branch may contain distinct historical implementations or test coverage. Compare the actual patch and test intent; do not blindly copy older files over the current architecture.

### `release-prep/fsm-rest-1.0.0`

The branch diff reports:

- `docs/RELEASE_1_0.md` — **not present on current development** at this snapshot.
- `docs/assets/release-frontier.svg` — **not present on current development** at this snapshot.
- Changes to the build workflow, README, MicroBundle documentation, project file, and descriptor-binding tests.
- Removal of `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs` from the core-project path.

The current architecture keeps the FSM_COS integration in the separate `src/FSM_REST.COS/` project, including its own `RestApiDescriptorExtensions.cs` and `RestMicroBundle.cs`. Do not restore those integration types into the core merely because the release branch had a different layout. Review `RELEASE_1_0.md` and `release-frontier.svg` for durable release criteria or useful visual explanations; harvest relevant material into current documentation if it remains accurate, then leave deletion to the owner.

### Release contract and frontier visual: reviewed

The unique `docs/RELEASE_1_0.md` provides a useful release-checklist skeleton: independent restore/build/test/coverage, XML documentation review, package dependency inspection, practical README examples, theory and visual boundary checks, and explicit publication gating. Those criteria should be retained as a checklist, but the branch document is **not** accepted as a current 1.0 release contract without revision: the current work is focused on the existing development line, and its release status/prerequisites must be revalidated against current package metadata and FSM_COS Alpha.7.

The unique `docs/assets/release-frontier.svg` illustrates capability recipe → operation descriptor → runtime binding → request/transport → current remote data, which is a helpful explanation of the capability-versus-response boundary. However, its labels assert `FSM_REST 1.0.0` and `FSM_COS 1.0.0 stable prerequisite`; those are historical branch claims, not verified current release facts. Do not promote the SVG as current release evidence unchanged. If reused, revise the labels and mark it conceptual or source-checked according to the actual generation process.

This review preserves the substantive value in the ledger without copying stale release claims into the active documentation.

## Manual completion checklist

- [ ] Inspect branch-only commits, not just the current changed-file summary.
- [ ] Compare candidate docs and SVGs against current `development`; retain accurate missing explanations.
- [ ] Compare test intent and edge cases; port missing tests into the current test project.
- [ ] Confirm no useful release criteria or package-boundary decisions remain only in branch history.
- [ ] Re-run CI after any harvested changes.
- [ ] Owner manually deletes only branches whose unique work has been reconciled.

**No publication or release action is authorized by this ledger.**
