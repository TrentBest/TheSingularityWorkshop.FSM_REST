# FSM_REST Documentation Readiness

**Owner / Workshop moniker:** @TrentBest  
**Scope:** Align this repository with the FSM_COS Workshop documentation standard.  
**Branch:** development  
**Policy:** Source-derived facts only. Label manual diagrams as manual. Documentation coverage is not correctness, runtime proof, or release readiness.

## Standard
- README follows the Workshop five-part opening and links to detailed documentation.
- Gallery.md provides a concise overview, working recipes, visual examples, provenance, and evidence.
- Every production .cs file has a neighboring companion .md covering author/moniker, purpose, responsibilities, boundaries, source, and tests.
- Each relevant class has a target-centered UML page under docs/UML with relationships and use cases.
- UML distinguishes expected consumer roles from source-observed relationships and labels manual vs generated provenance.
- Readiness claims identify exact branch SHA and CI evidence.
- Links, diagrams, examples, package boundaries, and claims are checked; publishing remains approval-gated.

## Baseline inventory
The development tree contains **21 production C# source files** across core REST, optional OpenAPI provider, optional FSM_COS adapter, and optional GUI adapter. Test sources are excluded.

| Area | State | Remaining work |
|---|---|---|
| README / five-part entry | Present | Audit claims and accessibility/readability |
| Gallery | Present | Check each visual against source and runnable recipe |
| Consumer guide | Present | Link per-class docs and UML index |
| Source companions | **21/21 present** | Review technical accuracy and verify links |
| UML index and architecture | Architecture diagrams grouped under `docs/UML/Architecture/`; FSM_API lifecycle diagram added | Audit every public type, relationships, use cases, and link-check findings |
| OpenAPI provider | Separate project/package; source companions and UML present | Verify fidelity warnings, package dependency graph, and consumer examples |
| GUI adapter | Companion and target-centered UML present | Verify emitted metadata and renderer limitations; keep execution independent of OpenAPI |
| FSM_COS adapter | Companion and target-centered UML present | Verify against current MicroBundleDomain contract |
| Visual provenance | Partially documented | Audit each SVG/recipe and classify evidence |
| Documentation CI | Source-companion contract, architecture guard, and recursive UML relative-link/image-path checker are wired into CI | Verify exact-SHA CI and resolve any link-check findings |
| Generated UML / GUI.SVG | Not claimed | Source-to-model generation remains future work |

## Production source inventory
All 20 previously inventoried production C# files plus `src/FSM_REST/RestFsmExecutor.cs` now have neighboring Markdown companions:
- src/FSM_REST/HttpClientRestTransport.cs
- src/FSM_REST/IRestTransport.cs
- src/FSM_REST/RestApiDescriptor.cs
- src/FSM_REST/RestGuiInputMapping.cs
- src/FSM_REST/RestGuiMapping.cs
- src/FSM_REST/RestOperationBinding.cs
- src/FSM_REST/RestOperationDescriptor.cs
- src/FSM_REST/RestOperationExecutor.cs
- src/FSM_REST/RestParameterDescriptor.cs
- src/FSM_REST/RestRequest.cs
- src/FSM_REST/RestRequestBodyDescriptor.cs
- src/FSM_REST/RestRequestFactory.cs
- src/FSM_REST/RestFsmExecutor.cs
- src/FSM_REST/RestResponse.cs
- src/FSM_REST/RestResponseDescriptor.cs
- src/FSM_REST.COS/RestApiDescriptorExtensions.cs
- src/FSM_REST.COS/RestMicroBundle.cs
- src/FSM_REST.OpenApi/OpenApiJsonImporter.cs
- src/FSM_REST.OpenApi/OpenApiRestApiConnection.cs
- src/FSM_REST.GUI/RestGuiNodeFactory.cs
- src/FSM_REST.GUI/RestGuiOperationExecutor.cs

## Published package baseline

The owner-provided NuGet Gallery snapshot confirms `TheSingularityWorkshop.FSM_Rest` version **`0.1.0-alpha.4`** is the current published version shown, targeting .NET 8.0. The repository candidate `0.1.0-alpha.5` is therefore an update candidate, not a first release. The same snapshot shows `TheSingularityWorkshop.FSM_API` `1.0.13` and `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`; verify dependency metadata from the packed artifacts before publication. NuGet publication remains explicitly approval-gated.

## Domain assessment / implementation status

The [Architecture Contract](ARCHITECTURE.md) now defines package boundaries and verification levels. A separate [Domain Gap Assessment](DOMAIN_GAP_ASSESSMENT.md) records implemented capabilities, explicit boundaries, and missing behavioral/semantic coverage. OpenAPI import and connection code has been moved into `src/FSM_REST.OpenApi`; GUI operation execution now lives in `src/FSM_REST.GUI` and accepts a neutral descriptor plus injected transport. This is intentionally not a readiness percentage. Initial P0 correctness work now rejects unsafe operation paths (absolute/network paths, query/fragment injection, and literal or percent-encoded dot segments) and malformed HTTP method tokens; regression tests cover those boundaries and query-value percent encoding. CI evidence must be recorded only after the latest implementation commit finishes.

## Latest exact-SHA verification evidence

- **Branch:** `development`
- **Verified commit:** [`140b15910f081f0b8b8699e26a42f1c26cc24543`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/140b15910f081f0b8b8699e26a42f1c26cc24543).
- **Workflow:** [Package FSM REST run 40069560306](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40069560306) — completed successfully on this exact SHA.
- **Build-and-test:** passed, including documentation contract, architecture boundary and UML link/image checks; restore/build/test with coverage, deterministic demo, NuGet pack, package audit, and artifact uploads passed.
- **Source integration:** passed, including restore/build/test against MicroBundleDomain repository sources.
- **NuGet publication:** skipped; no packages were published.
- **Artifacts:** package artifact `12551185011` (`fsm-rest-nuget-package`) and test-results artifact `12550672757`; both are unexpired and tied to this exact SHA.
- **Package archive SHA-256:** `cf1510ceefa5ce5fe21e8e72d222c22fad475ba6700e3b4b799411f61c0815da`.
- **Evidence scope:** this is the latest recorded successful CI snapshot. A later tracker-only commit must pass CI before the resulting repository head can be considered release-verified. The artifact is a CI-produced candidate; package contents still require release review and explicit publication approval.

## Current development delta (not yet CI-verified)

- The FSM-backed REST executor now has five source-level regression cases: HTTP 404 as a completed transport response, transport exception propagation, requested cancellation, unsolicited cancellation, and overlapping invocations returning their own responses. These tests are present in `tests/FSM_REST.Tests/RestFsmExecutorTests.cs`; their presence is not evidence that they pass.
- The executor now best-effort records `Failed`/`Cancelled` without replacing the original transport exception. A lifecycle-recording failure is attached under `FSM_REST.LifecycleRecordingException`; a cleanup failure during propagation is attached separately under `FSM_REST.CleanupException`, so one does not overwrite the other. If cleanup alone fails after success, it surfaces. These exception-preservation paths are source-reviewed but not yet tested or CI-verified. Deterministic fault-injection coverage for lifecycle-recording and `DestroyInstance` failures remains open.
- Recent implementation commits: [`ad61da04953f436fba0b35d219dde7b15a3612d6`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/ad61da04953f436fba0b35d219dde7b15a3612d6) adds best-effort terminal-state recording; [`dca8570d3b6cff3614472324471de7bd4bad6c7b`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/dca8570d3b6cff3614472324471de7bd4bad6c7b) separates lifecycle-recording and cleanup exception keys. The concurrency regression test was added in [`9af6af61688af5ec6b1509325ae6376cd64bc036`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/9af6af61688af5ec6b1509325ae6376cd64bc036). Recent companion/UML updates document the executor lifecycle, its source/test limits, and optional GUI integration boundaries. The latest UML clarification is [`03d8dcd2e30beef621c8f2958173a3c935b6d04c`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/03d8dcd2e30beef621c8f2958173a3c935b6d04c), which now distinguishes lifecycle-recording failures (`FSM_REST.LifecycleRecordingException`) from cleanup failures (`FSM_REST.CleanupException`) and states that deterministic fault-injection coverage remains open. These changes are not being represented as CI-verified; the historical exact-SHA evidence above does not cover them.

## Evidence and completion rule
Do not mark documentation 100% ready until production sources are reconciled against companions, every public class/record/interface has verified UML/use-case coverage, local links and examples are checked, and documentation/full CI passes at the exact SHA. A prior code CI pass at SHA 32e26834a2e6a9e7c3cfbe80ee64a9dff6e9d296 predates these documentation commits. A documentation contract check is now wired into the main build workflow; final verification must cite a successful run at the latest documentation SHA.

**Current documentation readiness: in progress (estimated 68%).** The source inventory is 21 production C# files and all 21 have neighboring companions. The public-type/UML index cross-check and Gallery source-link/claim review have now been performed against the `development` tree: the listed core and optional GUI, OpenAPI, and FSM_COS targets have corresponding UML pages; Gallery examples distinguish implemented behavior, manual explanatory visuals, and host responsibilities. This is a manual audit, not a generated completeness proof. Exact-SHA CI at `534cdc6bce2afa341e108328a35ac8dfd6da919e` passed the recursive documentation contract, architecture boundary check, UML links/image paths, build/test, deterministic demo, and package-artifact audit (run [40154265174](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40154265174)); NuGet publish was skipped. The detailed SVG audit then identified three fidelity issues and corrected them on `development`: the capability recipe had labeled the core description a “REST MicroBundle” even though `RestMicroBundle` belongs to the optional FSM_COS adapter; the execution flow called `IRestTransport` a “communication policy” despite the host-owned policy boundary; and the composition map visually implied FSM_REST consumes FSM_API rather than depending on FSM_API for its opt-in lifecycle. The labels and composition-map arrow direction now reflect the architecture contract. This is a manual visual-content audit, not generated SVG validation. Remaining work: verify the revised SVGs and their geometry/rendering, improve deterministic fault-injection coverage only with an appropriate non-public seam, and repeat exact-SHA CI after these edits. Do not treat the 21/21 companion count or a successful documentation gate as 100% release readiness.

## Next sequence
1. Complete the domain assessment and implement P0 correctness/contract gaps before treating documentation completeness as release readiness.
2. Preserve the OpenAPI provider boundary and test its package metadata, fidelity warnings, and consumer examples independently.
3. Audit all public types against target-centered UML pages, including same-file and auxiliary public declarations.
4. Audit Gallery visuals, recipes, and local Markdown links; distinguish implemented behavior, manual explanation, and proposed future behavior.
5. Run documentation checks and full CI; record exact SHA and evidence.
6. Keep NuGet publication approval-gated; do not publish as part of documentation work.


## FSM_API fault-injection feasibility audit (2026-10-25)

Source inspected on the FSM_API `development` ref:
- `FSM_API/FSM_API.Interaction.cs` exposes a static `DestroyInstance(FSMHandle)` entry point; the integration has no injected cleanup service or transport-like seam for substituting that call.
- `FSM_API/FSM_API.Internal.cs` removes the handle from its registered bucket and calls `handle.DestroyHandle()`. It catches `KeyNotFoundException` and reports that condition through the API's internal error event path; null or unregistered handles are also reported through that path rather than surfaced as a normal cleanup exception.
- `FSM_API/FSMHandle.cs` documents that `IsValid` mirrors `Context.IsValid`; it is not a destruction flag. A context made invalid is skipped by scheduling but remains registered until explicit destruction.

**Testing implication:** a normal, deterministic test that forces `DestroyInstance` to throw is not available through the current `RestFsmExecutor` dependency surface. Adding a production-only callback/delegate solely to manufacture this failure would enlarge the API surface for a test seam. Prefer first testing observable lifecycle outcomes and exception preservation with the current public API; if deterministic fault injection remains a release requirement, introduce a deliberately designed internal abstraction only if it can be kept out of the public package contract. The same constraint applies to terminal-state recording: the executor owns a concrete FSM_API handle and static API calls, so a test cannot simply substitute a throwing recorder.

This is a source-level feasibility assessment, **not** a test result and **not** evidence that exception paths are impossible. The REST implementation's secondary-exception handling remains untested until an appropriate seam or naturally reproducible failure is established. FSM_API source inspected at repository ref `development`; this note does not claim the REST package CI passed.

## UML coverage gate audit (2026-10-25)

- The UML index now links directly to [OpenApiImportResult](UML/OpenApiImportResult.UML.md), an optional OpenAPI provider result type that reports the normalized API descriptor and fidelity warnings. Index change: [`e1e015ae97f41c280cac642b2a25b3aca0064917`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/e1e015ae97f41c280cac642b2a25b3aca0064917).
- Audit found that `scripts/check_documentation.py` previously used a top-level-only glob for UML target pages, while this repository deliberately groups pages under nested `Architecture/`, `GUI/`, `OpenAPI/`, and `FSM_COS/` directories. The checker now walks `docs/UML` recursively and counts target pages recursively, excluding `README.md` navigation indexes at every level. Change: [`67282bd51794c96609b50882679b32c9484ba196`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/67282bd51794c96609b50882679b32c9484ba196).
- This is a gate-coverage improvement, not proof that every page passes the strengthened check. The recursive contract and link checks must run in CI against the current head; no new successful exact-SHA CI result is confirmed yet.
- **Readiness remains 68%.** Do not raise it based solely on adding an index link or broadening a validator. The next evidence step is to verify every nested target page satisfies the contract and inspect the complete Gallery/recipe/link surface, then record exact-SHA CI.


## Exact-SHA verification update (2026-10-25)

- Verified commit: [`534cdc6bce2afa341e108328a35ac8dfd6da919e`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/534cdc6bce2afa341e108328a35ac8dfd6da919e).
- Workflow: [40154265174](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40154265174). `build-and-test` and `source-integration` both succeeded. Documentation contract, architecture boundaries, UML relative links/image paths, restore/build/test, deterministic end-to-end demo, package packing, and packed-artifact audit all passed. `publish_nuget` was skipped.
- Manual coverage review cross-checked the production source inventory and UML index across core and optional integration folders; this confirms navigation coverage at the reviewed tree, not generated semantic completeness or runtime behavior.
- Gallery review confirmed its recipe examples and optional integration sections state the relevant source paths and boundaries; diagrams remain explanatory/manual, not runtime-observed or source-generated. A detailed SVG geometry/claim audit remains open.
- **Readiness remains 68%.** The successful run validates the current commit only; any later commit requires its own exact-SHA verification.
