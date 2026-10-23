# FSM_REST Documentation Readiness

**Owner / Workshop moniker:** @TrentBest  
**Scope:** Align this repository with the FSM_COS Workshop documentation standard.  
**Branch:** development  
**Policy:** Source-derived facts only. Label manual diagrams as manual. Documentation coverage is not correctness, runtime proof, or release readiness.

## Standard
- README follows the Workshop five-part opening and links to detailed documentation.
- Gallery.md provides a concise overview, working recipes, visual examples, provenance, and evidence.
- Every production .cs file has a neighboring companion .md covering author/moniker, purpose, responsibilities, boundaries, source, and tests.
- Each relevant class has a separate target-centered UML page under docs/UML with explicit relationships and use cases.
- UML distinguishes expected consumer roles from source-observed relationships and labels manual vs generated provenance.
- Readiness claims identify the exact branch SHA and CI evidence.
- Links, diagrams, examples, package boundaries, and claims are checked; publishing remains approval-gated.

## Baseline inventory
The current development tree contains **20 production C# source files** across core REST, optional FSM_COS adapter, and optional GUI adapter. Test sources are excluded from this per-production-file companion count.

| Area | State | Remaining work |
|---|---|---|
| README / five-part entry | Present | Audit claims and accessibility/readability |
| Gallery | Present | Check each visual against source and runnable recipe |
| Consumer guide | Present | Link per-class docs and UML index |
| UML index/core architecture | Partial | Add navigation index and reconcile class coverage |
| Core companions | Partial | Companion pages for every production source file |
| GUI adapter | In progress | Factory companion and target-centered UML added in this batch |
| FSM_COS adapter | Partial | Document adapter classes and boundary |
| Readiness evidence | Started | Run local link/path checks and record exact SHA + CI |
| Generated UML / GUI.SVG | Not claimed | Current diagrams are manually authored; generated promotion is future work |

## Production source inventory
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

Every listed file needs a neighboring Markdown companion. Each public class, record, or interface should have focused UML/use-case coverage or an explicit rationale for inclusion in a cohesive diagram.

## Evidence and completion rule
Do not mark documentation 100% ready until production sources are reconciled against neighboring Markdown, each class's UML/use-case coverage is checked, links and examples are validated, and current exact-SHA CI passes. The previously passing code CI is for SHA 32e26834a2e6a9e7c3cfbe80ee64a9dff6e9d296; it does not verify documentation changes added after that SHA.

**Current documentation readiness: in progress.** Percentage intentionally withheld until the checklist denominator is reconciled and evidence is collected.

## Next sequence
1. Finish companions, prioritizing core and FSM_COS integration boundaries.
2. Complete target-centered UML and use-case pages for each production class, grouping closely related value records only when each target remains explicit.
3. Add docs/UML/README.md as the navigation index.
4. Audit Gallery visuals/recipes against source and label each visual implemented, manual conceptual, or proposed.
5. Add CI checks for missing companions, missing UML/use-case coverage, and broken local links where robust.
6. Re-run documentation and full CI; record exact SHA and evidence.