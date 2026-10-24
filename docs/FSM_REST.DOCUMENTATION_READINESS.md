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
The development tree contains **20 production C# source files** across core REST, optional FSM_COS adapter, and optional GUI adapter. Test sources are excluded.

| Area | State | Remaining work |
|---|---|---|
| README / five-part entry | Present | Audit claims and accessibility/readability |
| Gallery | Present | Check each visual against source and runnable recipe |
| Consumer guide | Present | Link per-class docs and UML index |
| Source companions | **20/20 present** | Review technical accuracy and verify links |
| UML index and architecture | Expanded | Audit every public type, relationships, and use cases |
| GUI adapter | Companion and target-centered UML present | Verify emitted metadata and renderer limitations |
| FSM_COS adapter | Companion and target-centered UML present | Verify against current MicroBundleDomain contract |
| Visual provenance | Partially documented | Audit each SVG/recipe and classify evidence |
| Documentation CI | Minimum contract checker added; first enforcement run exposed three legacy UML metadata gaps, now patched | Verify the patched pages pass; extend checks only where robust, including local links |
| Generated UML / GUI.SVG | Not claimed | Source-to-model generation remains future work |

## Production source inventory
All 20 listed production C# files now have neighboring Markdown companions:
- src/FSM_REST/HttpClientRestTransport.cs
- src/FSM_REST/IRestTransport.cs
- src/FSM_REST/OpenApiJsonImporter.cs
- src/FSM_REST/OpenApiRestApiConnection.cs
- src/FSM_REST/RestApiDescriptor.cs
- src/FSM_REST/RestGuiInputMapping.cs
- src/FSM_REST/RestGuiMapping.cs
- src/FSM_REST/RestGuiOperationExecutor.cs
- src/FSM_REST/RestOperationBinding.cs
- src/FSM_REST/RestOperationDescriptor.cs
- src/FSM_REST/RestOperationExecutor.cs
- src/FSM_REST/RestParameterDescriptor.cs
- src/FSM_REST/RestRequest.cs
- src/FSM_REST/RestRequestBodyDescriptor.cs
- src/FSM_REST/RestRequestFactory.cs
- src/FSM_REST/RestResponse.cs
- src/FSM_REST/RestResponseDescriptor.cs
- src/FSM_REST.COS/RestApiDescriptorExtensions.cs
- src/FSM_REST.COS/RestMicroBundle.cs
- src/FSM_REST.GUI/RestGuiNodeFactory.cs

## Domain assessment / implementation status

A separate [Domain Gap Assessment](DOMAIN_GAP_ASSESSMENT.md) now records implemented capabilities, explicit boundaries, and the missing behavioral/semantic coverage. This is intentionally not a readiness percentage. Initial P0 correctness work now rejects unsafe operation paths (absolute/network paths, query/fragment injection, and literal or percent-encoded dot segments) and malformed HTTP method tokens; regression tests cover those boundaries and query-value percent encoding. CI evidence must be recorded only after the latest implementation commit finishes.

## Evidence and completion rule
Do not mark documentation 100% ready until production sources are reconciled against companions, every public class/record/interface has verified UML/use-case coverage, local links and examples are checked, and documentation/full CI passes at the exact SHA. A prior code CI pass at SHA 32e26834a2e6a9e7c3cfbe80ee64a9dff6e9d296 predates these documentation commits. A documentation contract check is now wired into the main build workflow; final verification must cite a successful run at the latest documentation SHA.

**Current documentation readiness: in progress.** Percentage withheld until the remaining UML denominator, link audit, visual provenance, and CI evidence are reconciled.

## Next sequence
1. Complete the domain assessment and implement P0 correctness/contract gaps before treating documentation completeness as release readiness.
2. Reconcile the OpenAPI importer/connection placement with the previously agreed provider boundary; test the optional package independently.
3. Audit all public types against target-centered UML pages, including same-file and auxiliary public declarations.
4. Audit Gallery visuals, recipes, and local Markdown links; distinguish implemented behavior, manual explanation, and proposed future behavior.
5. Run documentation checks and full CI; record exact SHA and evidence.
6. Keep NuGet publication approval-gated; do not publish as part of documentation work.
