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

- **Verified branch head at inspection:** `development`, commit [`cb03e7e049da391d11cc7accdc9a8acf0e609610`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/cb03e7e049da391d11cc7accdc9a8acf0e609610).
- **CI:** [Package FSM REST run #40164358155](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40164358155) completed successfully against that exact SHA. Build/test and source-integration passed; `publish_nuget` was skipped.
- **Tests:** 71 passed, 0 failed in the test-results artifact associated with that run.
- **Package archive:** artifact `12587164237`, `fsm-rest-nuget-package.zip`; SHA-256 `cabe7184ad278d9bd765bfa803abd8495f7386f68b4a642140a71925320dc7d8`.
- **Core package:** `TheSingularityWorkshop.FSM_Rest.0.1.0-alpha.5.nupkg`; SHA-256 `ffa0c85a9193873238dab239a2ac6197605a8071f870aa27774b82bfb86f20e2`.
- **OpenAPI package:** `TheSingularityWorkshop.FSM_Rest.OpenApi.0.1.0-alpha.1.nupkg`; SHA-256 `4b9cee5b30d8df344f5ad4f585a4f5d6fcc39d2755150f858d72ddd79279156d`.
- **FSM_COS adapter package:** `TheSingularityWorkshop.FSM_Rest.COS.0.1.0-alpha.1.nupkg`; SHA-256 `8637b86778ccc439bbc6425572df5aae70f986358ee998ca3656903c99ce33d0`.
- **Artifact review:** package nuspecs identify the exact commit above; core depends on FSM_API `1.0.13` without a MicroBundleDomain dependency, OpenAPI depends on core alpha.5, and COS depends on core alpha.5 plus MicroBundleDomain `1.0.1`. All three packaged READMEs were checked; no unresolved local links were found.
- **Evidence boundary:** these results verify the candidate at the stated SHA only. Any subsequent documentation or source commit needs its own successful exact-SHA CI run and candidate-artifact inspection before it can become final release evidence.

## Remaining documentation and release gaps

- **Historical documentation readiness estimate (2026-10-25): 68%.** This was a human review estimate, not test coverage or release readiness, and is not a current score.
- **Reader experience:** the README now links to the shared FSM_COS documentation and visual standards and uses the shared five-part entry journey. Continue simplifying duplicated explanations and ensure every image teaches one identifiable idea.
- **Visual fidelity and provenance:** audit each SVG against current source, give every visual a clear caption/text equivalent, and distinguish conceptual, source-checked, generated, and runtime-observed material. UML pages remain manually authored/source-checked unless a generation pipeline is explicitly demonstrated.
- **Public contract review:** inspect XML docs, consumer examples, and package-level guidance against the exact candidate API.
- **Exception-path coverage:** deterministic fault-injection tests for lifecycle-recording and FSM instance cleanup failures remain open. The five executor regression tests (HTTP non-success response, transport failure, requested cancellation, unrequested cancellation, and concurrent response independence) are covered by the latest 71/71 CI run; that does not close the separate fault-injection gap.
- **Release owner review:** release notes and all unchecked checklist items still require review. Publication remains disabled until explicitly approved.

## Evidence and completion rule
Do not mark documentation 100% ready until production sources are reconciled against companions, every public class/record/interface has verified UML/use-case coverage, local links and examples are checked, and documentation/full CI passes at the exact SHA. A prior code CI pass at SHA 32e26834a2e6a9e7c3cfbe80ee64a9dff6e9d296 predates these documentation commits. A documentation contract check is now wired into the main build workflow; final verification must cite a successful run at the latest documentation SHA.

**Documentation readiness: in progress.** The 68% estimate in this document is historical (2026-10-25), not a current release-readiness score. Recalculate only after the current-head source, UML, links, examples, package boundaries, and release evidence have been audited. The source inventory is 21 production C# files and all 21 have neighboring companions. The public-type/UML index cross-check and Gallery source-link/claim review have now been performed against the `development` tree: the listed core and optional GUI, OpenAPI, and FSM_COS targets have corresponding UML pages; Gallery examples distinguish implemented behavior, manual explanatory visuals, and host responsibilities. This is a manual audit, not a generated completeness proof. Exact-SHA CI at `534cdc6bce2afa341e108328a35ac8dfd6da919e` passed the recursive documentation contract, architecture boundary check, UML links/image paths, build/test, deterministic demo, and package-artifact audit (run [40154265174](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40154265174)); NuGet publish was skipped. The detailed SVG audit then identified three fidelity issues and corrected them on `development`: the capability recipe had labeled the core description a “REST MicroBundle” even though `RestMicroBundle` belongs to the optional FSM_COS adapter; the execution flow called `IRestTransport` a “communication policy” despite the host-owned policy boundary; and the composition map visually implied FSM_REST consumes FSM_API rather than depending on FSM_API for its opt-in lifecycle. The labels and composition-map arrow direction now reflect the architecture contract. This is a manual visual-content audit, not generated SVG validation. Remaining work: verify the revised SVGs and their geometry/rendering, improve deterministic fault-injection coverage only with an appropriate non-public seam, and repeat exact-SHA CI after these edits. Do not treat the 21/21 companion count or a successful documentation gate as 100% release readiness.

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
- **The historical 68% estimate is not a current score; recompute after the current-head audit and exact-SHA CI.** Do not raise it based solely on adding an index link or broadening a validator. The next evidence step is to verify every nested target page satisfies the contract and inspect the complete Gallery/recipe/link surface, then record exact-SHA CI.


## Exact-SHA verification update (2026-10-25)

- Verified commit: [`534cdc6bce2afa341e108328a35ac8dfd6da919e`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/534cdc6bce2afa341e108328a35ac8dfd6da919e).
- Workflow: [40154265174](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40154265174). `build-and-test` and `source-integration` both succeeded. Documentation contract, architecture boundaries, UML relative links/image paths, restore/build/test, deterministic end-to-end demo, package packing, and packed-artifact audit all passed. `publish_nuget` was skipped.
- Manual coverage review cross-checked the production source inventory and UML index across core and optional integration folders; this confirms navigation coverage at the reviewed tree, not generated semantic completeness or runtime behavior.
- Gallery review confirmed its recipe examples and optional integration sections state the relevant source paths and boundaries; diagrams remain explanatory/manual, not runtime-observed or source-generated. A detailed SVG geometry/claim audit remains open.
- **The historical 68% estimate is not a current score.** The successful run validates the current commit only; any later commit requires its own exact-SHA verification.


## CoderLegion real-service sample (2026-10-25)

- Added `samples/CoderLegionArticleLibrary`, a .NET 8 read-only console sample using FSM_REST operation descriptors, `RestOperationExecutor`, and `IRestTransport`.
- Supported operations: paginated `GET /posts?page=N` and full-detail `GET /posts/{id}`, based on the CoderLegion API contract supplied in the work session.
- Credentials are read from `CODERLEGION_API_KEY`; the sample recommends a Personal API Key and does not embed credentials. Optional `CODERLEGION_HANDLE` filters results client-side because the supplied contract describes `GET /posts` as a browse endpoint, not a dedicated own-posts listing.
- The Gallery links the recipe. This is a live, credential-gated integration example; it has **not** yet been run against CoderLegion, so endpoint behavior and account-specific access remain unverified.
- Write operations are intentionally excluded. Next increment after read verification: local draft save/preview, create-options metadata, then user-confirmed create/edit calls with no automatic publishing.
- This feature addition does not by itself establish release readiness. Recalculate the historical 68% estimate only after the current-head audit and exact-SHA CI.

- The main `build-and-test` workflow now compiles the CoderLegion sample without making network requests or requiring credentials. This validates compilation only; the live API calls remain a manual, Personal-Key-gated check.


## Parameter contract follow-up (2026-10-27)

- Implemented and regression-tested missing/null versus explicit-empty query values, requiredness behavior, plus-sign percent encoding, empty path rejection, and duplicate/ambiguous parameter-name rejection.
- The optional OpenAPI importer now reports explicit style/explode and array/object serialization gaps and skips unsupported parameter locations with a warning.
- Exact-SHA CI is required for the current head after these additions. The prior artifact hashes and 2026-10-25 evidence above are historical and must not be used as final release evidence.


## Exact-SHA verification update (2026-10-27, packaging cleanup)

- **Verified branch:** `development`, commit [`f134170f93b4eaa2f315f915412e574e6799d697`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/f134170f93b4eaa2f315f915412e574e6799d697).
- **CI:** [Package FSM REST run #40445151564](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40445151564) completed successfully at that exact SHA. Both `build-and-test` and `source-integration` succeeded; `publish_nuget` was skipped.
- **Checks:** documentation contract, architecture boundaries, recursive UML links/image paths, restore/build/tests with coverage, deterministic end-to-end demo, storefront composition proof, CoderLegion sample build, package packing, and packed-artifact audit all passed.
- **Package artifact:** workflow artifact [`fsm-rest-nuget-package`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40445151564) was created; GitHub reports SHA-256 `e7f427e3359fb7c680a64695ff778d12587cc93a78a7ae80e5b38ef9489508c5` for the uploaded ZIP artifact.
- **Scope of evidence:** this proves CI's package-content and dependency-boundary assertions passed for this SHA. It does not replace a separate manual review of the downloadable archive, consumer examples, SVG rendering/fidelity, or remaining behavior gaps.
- The packaging cleanup excludes the four internal readiness/reconciliation/checklist documents from the core and OpenAPI packages, removes the checklist from public README navigation, and makes CI fail if those internal documents leak into packed artifacts. Consumer documentation and UML remain eligible for package inclusion.
- Package versions remain unchanged: core `0.1.0-alpha.5`, OpenAPI `0.1.0-alpha.1`, and COS `0.1.0-alpha.1`. No NuGet publication occurred.

