<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Branch Reconciliation Ledger

**Snapshot date:** 2026-11-06  
**Comparison base:** `development` at `49bcc764f416deca5cff589143a3da4988fa961d`  
**Purpose:** Reconcile the live branch inventory and preserve unique work before the repository owner manually removes surplus branches. This ledger is evidence, not authorization to delete, merge, release, or publish.

> **Sith policy:** `development` is the active work branch; `master` is the released baseline. The desired steady state is exactly those two branches. The repository owner performs branch deletion manually after review. No branch was deleted during this reconciliation.

## Live branch inventory

GitHub's live branch search returned exactly four branches:

| Branch | Relationship to development | Current disposition |
|---|---|---|
| `master` | Protected/released baseline; not compared as a cleanup candidate | Keep. Do not modify as part of this work. |
| `development` | Active baseline, SHA `49bcc764f416deca5cff589143a3da4988fa961d` | Keep. All implementation work targets this branch. |
| `docs/rest-textbook-theory` | 51 commits ahead, 0 behind; merge base is the current development SHA | Active head for open draft PR [#17](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/pull/17). Do not delete or merge as part of branch cleanup. |
| `release-prep/fsm-rest-1.0.0` | 13 commits ahead, 571 behind; merge base `6c1f52ae4f614a63b1cccc82fc498e4d13667641` | Historical candidate. Unique release checklist intent is preserved in `docs/RELEASE_CHECKLIST.md`; the old release contract and visual are superseded/outdated. Complete final branch-only commit audit before marking ready for owner deletion. |

The branch inventory above supersedes older snapshots in this document's history and issue #16. The live endpoint response, not an earlier issue body or historical table, is the current inventory evidence.

## Open pull requests and reconciliation

### PR #17 — instructor-editable REST textbook and living SVG lessons

- [PR #17](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/pull/17) is open, draft, and unmerged.
- Head: `docs/rest-textbook-theory` at `dd3b28ca11e60d74c020601b5bf26f52385870a0`.
- Base: `development` at `49bcc764f416deca5cff589143a3da4988fa961d`; the branch compare confirms 51 commits ahead and zero behind.
- Exact-head workflow [run 42135058047](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/42135058047) passed `build-and-test` and `source-integration`. The Sith publication-policy job was skipped for the PR event and `publish_nuget` was skipped. No package was published.
- The branch adds four textbook chapters, conceptual SVG teaching assets, a source-anchor contract diagram/checker, a repository documentation checklist, and README/theory links. Its diagrams disclose their evidence level; the authored diagram checker is not a general C# parser or inferred dependency graph.
- Branding CI validates changed Markdown files. Untouched legacy Markdown remains a gradual migration backlog; repository-wide branding compliance is not claimed.
- Disposition: retain the branch while PR #17 is reviewed. No merge or branch deletion is authorized here.

### PR #14 — HTTP client builder/factory integration

- [PR #14](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/pull/14) is closed and merged as of 2026-11-05.
- Its reconciliation record identifies owner-authorized merge commit `a394194dbda581b679fd6310f4e028967ee63b45`; the exact-head workflow [run 41985535239](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/41985535239) passed build/test and source-integration, with NuGet publication skipped.
- This work separates the framework-neutral core HttpClient builder from the optional `IHttpClientFactory`/DI integration package. The old branch ledger and issue #15 text that describe PR #14 as open/unmerged are stale and must not be treated as current status.

## Historical release branch review

Comparison of `development...release-prep/fsm-rest-1.0.0` reports 13 branch-only commits and 571 commits behind development. The file comparison shows:

- `docs/RELEASE_1_0.md` and `docs/assets/release-frontier.svg` are present on the historical branch. The checklist's durable criteria—restore/build/test/coverage, XML documentation, package contents/dependencies, practical examples, architecture boundaries, exact-SHA CI, and explicit owner approval—are already represented in the active `docs/RELEASE_CHECKLIST.md`.
- The old release contract repeats current architecture boundaries but asserts a 1.0.0 release contract. The SVG labels FSM_REST 1.0.0 and FSM_COS 1.0.0 as a stable prerequisite. Those version/stability claims are not verified current facts; do not copy the SVG without correcting its labels and evidence status.
- The historical branch removes `src/FSM_REST/RestApiDescriptorExtensions.cs` and `src/FSM_REST/RestMicroBundle.cs` from the core-project path. The current development architecture keeps the optional FSM_COS integration in `src/FSM_REST.COS/`; do not restore historical adapters into core.
- The old release checklist is not proof of current package readiness. Its own active counterpart explicitly says the historical artifact inspection is superseded and requires fresh exact-SHA CI and artifact inspection.

**Disposition:** no known release-checklist requirement is currently stranded on this branch, but the 13 branch-only commits have not yet been exhaustively inspected one by one. Do not mark this branch ready for deletion until that final history audit is complete. Do not merge it.

## Current architectural and release invariants

- Core FSM_REST remains framework-neutral; OpenAPI, FSM_COS, GUI, hosting, and other integrations remain optional.
- `IRestTransport` is the shared runtime transport boundary. Foundry/host workflow sequencing remains outside FSM_REST.
- An HTTP non-success status remains an observed response, not automatically a transport exception or workflow failure.
- Authentication, credentials, retries, caching, rate limits, and application/domain policy remain host or optional-adapter concerns unless a concrete shared contract is justified.
- Package versions configured in source are not proof of NuGet publication. Packing and auditing are separate from publication.
- No NuGet publication, tag, deployment, force-push, merge, or branch deletion is authorized by this ledger.

## Manual completion checklist

- [ ] Review PR #17's complete diff against the current development baseline; retain the branch until its review/merge decision is made separately.
- [ ] Inspect all 13 branch-only commits on `release-prep/fsm-rest-1.0.0`, confirming no unique source, test, or accurate release requirement remains unpreserved.
- [ ] If the historical release branch passes that audit, mark it ready for the owner to delete manually.
- [ ] Keep the live branch inventory at four until the owner removes branches; re-query it before any publication gate.
- [ ] Run and record exact-SHA CI after this ledger update.
- [ ] Verify current package versions, target frameworks, dependencies, and packed artifacts before any separately authorized release review.

**No publication, release, merge, force-push, or branch deletion is authorized by this ledger.**

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
