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

## Exact-SHA verification evidence

- **Verified branch:** `development`
- **Verified branch:** `development`
- **Verified commit:** [`5154960223d230a92e0e7166e20f56ec4c51299b`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/commit/5154960223d230a92e0e7166e20f56ec4c51299b)
- **Workflow:** [Package FSM REST run 40061158845](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40061158845) — completed successfully at this exact SHA.
- **Build-and-test job:** passed.
- **Source-integration job:** passed.
- **NuGet publication job:** skipped; no packages were published.

This exact-SHA run validates the branch-reconciliation documentation update against the repository's build/test and source-integration workflows. The current readiness tracker update will itself create a newer commit, so that new SHA must complete CI before it is treated as current evidence.

## Evidence and completion rule
Do not mark documentation 100% ready until production sources are reconciled against companions, every public class/record/interface has verified UML/use-case coverage, local links and examples are checked, and documentation/full CI passes at the exact SHA. A prior code CI pass at SHA 32e26834a2e6a9e7c3cfbe80ee64a9dff6e9d296 predates these documentation commits. A documentation contract check is now wired into the main build workflow; final verification must cite a successful run at the latest documentation SHA.

**Current documentation readiness: in progress (estimated 68%).** The source inventory is 21 production files and all 21 have neighboring companions; architecture diagrams have a dedicated folder, and local UML links are enforced in CI. Remaining work: reconcile every public type to a target-centered UML page, audit visual provenance and technical fidelity of companion pages, and repeat exact-SHA CI after subsequent changes. The UML relative-link/image-path checker passed at the historical verified commit noted above, not necessarily at the current development head. Do not treat the 21/21 companion count as a completeness or correctness percentage.

## Next sequence
1. Complete the domain assessment and implement P0 correctness/contract gaps before treating documentation completeness as release readiness.
2. Preserve the OpenAPI provider boundary and test its package metadata, fidelity warnings, and consumer examples independently.
3. Audit all public types against target-centered UML pages, including same-file and auxiliary public declarations.
4. Audit Gallery visuals, recipes, and local Markdown links; distinguish implemented behavior, manual explanation, and proposed future behavior.
5. Run documentation checks and full CI; record exact SHA and evidence.
6. Keep NuGet publication approval-gated; do not publish as part of documentation work.
