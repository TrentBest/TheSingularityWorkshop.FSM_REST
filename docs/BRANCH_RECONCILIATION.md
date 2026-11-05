<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_REST Branch Reconciliation Ledger

**Live snapshot:** 2026-11-05 (GitHub branch enumeration)  
**Comparison base:** `development` at `49bcc764f416deca5cff589143a3da4988fa961d`  
**Documentation integration head:** `1f0df722be8c028cfc4a8742215b8eeff60d11b8` (PR #17; two-parent reconciliation commit)  
**Exact-head CI:** [Package FSM REST run 41998753547](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/41998753547) — `build-and-test` passed, `source-integration` passed; policy and NuGet publication skipped on the PR event.  
**Purpose:** Preserve unique work before any surplus branch is manually removed. This ledger is a review aid, not authorization to delete branches.

> **Sith policy:** the permanent branch set is exactly `master` and `development`. The repository owner performs surplus branch deletion manually only after reconciliation. No branch is deleted by this ledger or by the assistant.

## Current live branch inventory

GitHub returned exactly these six branches:

| Branch | Role / current disposition |
|---|---|
| `master` | Permanent released baseline. |
| `development` | Permanent active development branch; comparison base. |
| `docs/rest-textbook-theory` | Active documentation PR #17; reconciled with current development and validated at the exact head above. Do not delete before the PR is resolved. |
| `forge/fsm-rest-documentation-and-capability-flow` | Historical candidate; 16 commits ahead / 586 behind development. |
| `forge/fsm-rest-functional-standard` | Historical candidate; 14 commits ahead / 587 behind development. |
| `release-prep/fsm-rest-1.0.0` | Historical candidate; 13 commits ahead / 571 behind development. |

The three historical comparisons were freshly queried against `development` on 2026-11-05. Their commit counts are history comparisons, not proof that every changed file is unique or that deletion is authorized.

## Historical branch review

### `forge/fsm-rest-documentation-and-capability-flow`

The prior review found its major documentation, SVG, request-factory, and test paths already represented in current development. Its remaining 16 branch-only commits should be regarded as historical review material; no wholesale overlay is appropriate. Current comparison: 16 ahead / 586 behind.

### `forge/fsm-rest-functional-standard`

The prior review found substantial overlap with the active core descriptor, transport, workflow, and test implementation. A previously compared transport test file was identical to development at that review point. Current comparison: 14 ahead / 587 behind. Do not restore historical source snapshots over active code.

### `release-prep/fsm-rest-1.0.0`

The previous review harvested durable release criteria into `docs/RELEASE_CHECKLIST.md`. The historical branch's release-frontier visual carried unverified 1.0.0 and stable-prerequisite claims and should not be reused without correcting its claims and provenance. Its old placement of COS adapters under the core project is superseded by `src/FSM_REST.COS/`. Current comparison: 13 ahead / 571 behind.

## Current documentation integration

PR [#17](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/pull/17) adds the living REST textbook, four theory chapters, teaching SVGs, the documentation checklist, and a source-anchor-checked REST contract diagram. The integration commit has both the original textbook branch and current development as parents; its tree was based on current development before the textbook additions were applied.

The exact-head CI run passed the documentation contract, diagram anchor check, architecture checks, UML link checks, build, tests, deterministic samples, package audit, and source-integrated build/tests. The PR remains open and draft. The workflow does not publish NuGet packages on this PR event.

**Branding validation caveat:** `scripts/verify_markdown_branding.py` exists, but the current `.github/workflows/build.yml` does not yet invoke it. Do not treat repository-wide branding verification as a CI-enforced gate until that step is added and a full run passes. The shared canonical assets remain:
- Moniker: `https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg`
- Footer PNG: `https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png`

## Sith policy status and owner action


The permanent branch policy is currently **NON-COMPLIANT**: six live branches exist, not two. The three historical candidates above are ready for final owner-side review. After you manually remove those three historical branches, the expected remaining set is `master`, `development`, and the still-active PR branch `docs/rest-textbook-theory`; that PR branch must be resolved before the repository can reach the exact two-branch state.

The Sith policy workflow run [41998747731](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/41998747731) failed closed because the live branch count was not exactly two. This is expected enforcement, not a code/test failure.

## Release boundary

Current development package versions and publication status must be checked independently at the release gate. Build, test, pack, artifact upload, and package audit are not publication. NuGet publication remains prohibited unless the repository owner explicitly authorizes it and the live branch gate passes.

## Manual completion checklist

- [ ] Owner reviews and manually deletes the three historical branches only after deciding their branch-only history is fully reconciled.
- [ ] Resolve PR #17; do not delete its active branch before the PR is merged or closed.
- [ ] Re-run exact-head CI after any further change.
- [ ] Finish repository-wide Markdown branding reconciliation, enable the branding verifier in CI, and validate it before claiming the standard is enforced.
- [ ] Confirm the live remote branch set is exactly `master` and `development` before any authorized publication workflow.
- [ ] Obtain explicit owner authorization before NuGet publication.

**No branch deletion, PR merge, tag, deployment, force-push, or NuGet publication is authorized by this ledger.**

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
