# TheLLM.md — Integration Brief for The Architect

**Audience:** The LLM implementing the application in `TrentBest/The`  
**Authoring repository:** `TrentBest/TheSingularityWorkshop.FSM_REST`  
**Target application:** **The Architect** (WPF), repository `TrentBest/The`  
**Goal:** Make the user's own CoderLegion articles and the statistics actually available from the CoderLegion service visible inside The Architect, and use that as a real integration test of FSM_REST.

This is a coordination document, not a request to transfer ownership of The. The LLM working in `TrentBest/The` remains responsible for that application's implementation. Use this document as the integration contract and report back any mismatch between the app, the live API, and FSM_REST.

## 1. Product outcome

The user should be able to open The Architect and inspect their own CoderLegion authoring history without switching to a terminal or manually copying API responses.

The first useful screen should let the user:

- See a list of their articles, including the fields actually returned by the API (at minimum, where available: post ID, title, author handle, publication/creation date, and short description).
- Select an article and read its full Markdown body.
- See article-level statistics and/or author/account-level statistics **only when the API contract and live response confirm those metrics exist**. Examples to investigate include views, reactions/likes, comments, publication status, and engagement counts; do not invent or locally estimate server metrics.
- Refresh data and see loading, empty, stale, and error states clearly.
- Select an article so that it becomes semantic focus in The Architect and can later be included in TerminalAi context.
- Distinguish “not returned by this endpoint” from a true zero. Missing statistics are **unknown/not available**, not zero.

The result should be a usable application feature, not just a console dump or a static mockup.

## 2. Integration boundary

Use FSM_REST as the reusable REST capability/request/transport layer. Keep CoderLegion-specific JSON envelope parsing and application view models in The (or a separate The-owned adapter), not inside FSM_REST core.

Expected direction:

```text
CoderLegion API
      |
      v
FSM_REST capability descriptors
      |
      v
request binding / request construction
      |
      v
IRestTransport (HTTP implementation)
      |
      v
RestResponse (status, headers, body)
      |
      v
The-owned CoderLegion adapter
      |
      v
typed article + statistics view models
      |
      v
The Architect UI / semantic focus / TerminalAi
```

Important boundaries:

- FSM_REST core already directly references FSM_API; The should consume FSM_REST through its public API rather than copy its implementation.
- Do not add a dependency from FSM_REST core to The, WPF, CoderLegion-specific models, FSM_COS, or GUI frameworks.
- Keep authentication and policy in The's host/service boundary. Pass the Personal API Key as an `X-API-Key` header; never hard-code it, put it in source control, expose it in logs, or put it on a command line.
- Reuse a host-managed `HttpClient`/transport for the service lifetime. Do not create a new client for every row or refresh.
- Respect cancellation and avoid blocking the WPF dispatcher during network I/O.
- Handle non-2xx status codes explicitly. A transport returning an HTTP response is not the same thing as the endpoint succeeding; FSM_REST's lifecycle `Completed` means a response was returned, not that its status was successful.
- Keep API response models separate from UI view models where that improves clarity.
- Do not add unverified endpoints or undocumented fields merely to make a dashboard look complete.

## 3. Start with the existing verified read path

FSM_REST has a read-only sample in:

- [Sample README](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/samples/CoderLegionArticleLibrary/README.md)
- [Sample source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/tree/development/samples/CoderLegionArticleLibrary)
- [FSM_REST Gallery](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/Gallery.md)

The current documented base URL is `https://coderlegion.com/api/v1`. The initial sample describes these read endpoints:

- `GET /posts?page=N` — paginated browse feed.
- `GET /posts/{id}` — full post detail, including Markdown content when returned by the API.

The browse endpoint is not documented as a dedicated “my posts” endpoint. The sample filters returned results locally by `author.handle` when `CODERLEGION_HANDLE` is configured, and it caps the page scan. Therefore:

1. Verify the actual current response envelope and pagination rules against the API documentation and a credential-authorized request.
2. Filter by the authenticated user's known author handle only if the response schema supports it. Prefer a dedicated “my articles” endpoint if one is confirmed by the authoritative API contract.
3. Clearly disclose pagination limits; do not label a partial scan as the complete article library.
4. Fetch article details on selection or through a deliberate cache policy rather than downloading every full body during initial listing.
5. Add tests with representative, sanitized response fixtures and error envelopes.

A credential-free build/test path must remain available. CI should compile and test using fake transports/fixtures without calling CoderLegion or requiring a secret.

## 4. Article statistics: verify before implementing

The user's goal includes seeing article statistics. This is a required discovery task, not permission to invent an endpoint.

Inspect the current CoderLegion API documentation, existing API contract material, and actual authorized responses to establish:

- Which article-level metrics are exposed.
- Whether metrics appear in `GET /posts`, `GET /posts/{id}`, a separate analytics endpoint, or more than one response.
- Whether metrics are public, author-only, or permission-gated.
- Whether metrics are current snapshots or time-series data.
- Whether there is an author/account overview endpoint.
- Pagination, rate limits, missing-value semantics, and error responses.

Document each metric with its **source endpoint, exact response field, meaning, unit, and retrieval time**. Preserve unknown values as unknown. If an endpoint is not documented or authorized, do not probe arbitrary write/admin routes; request the proper documentation or use the read endpoints that are confirmed.

Build the UI around metrics the service actually returns. If only a subset is available, show that subset and label it accurately. If statistics are not exposed to this API key, surface that fact as a capability gap rather than fabricating values.

## 5. Suggested application structure

Adapt to The's existing architecture rather than imposing a parallel framework. A reasonable separation is:

- **CoderLegion client/service:** owns base URL, host-managed transport, authentication header, cancellation, paging, and status/error translation.
- **API DTOs:** represent the response envelope and fields exactly as returned.
- **Application models:** article summary, article detail, and confirmed statistics; avoid coupling WPF controls directly to raw JSON.
- **View models/state:** articles collection, selected article, statistics, loading/refresh state, error state, last refreshed time.
- **WPF view:** a readable article list/detail surface with an honest statistics panel.
- **Semantic focus adapter:** exposes selected article identity/title and relevant metadata to The's existing focus/TerminalAi mechanism.

Keep the first slice small and end-to-end: list → select → detail → confirmed stats → refresh. Do not block this first working path on a generalized publishing studio.

## 6. Security and user control

- Read the Personal API Key from an existing secure settings mechanism if The already has one; otherwise use a documented environment-variable/development-secret approach and recommend an appropriate secure store for normal use.
- Never print the key, include it in exception text, telemetry, copied TerminalAi context, screenshots, or diagnostics.
- Do not use a Master API Key for ordinary local article browsing.
- Do not persist the key in repository files or article caches.
- Keep all operations read-only for this milestone. No create, edit, delete, upload, or publish action should be implemented as part of the article/statistics viewer.
- If a later milestone adds writes, require a separate explicit preview-and-confirm action, show the exact target and content, and treat publishing as a potentially irreversible remote side effect.

## 7. Testing plan

### Automated tests (no credentials/network)

Use a fake `IRestTransport` or the existing FSM_REST test seam to cover:

- successful paginated listing and correct page termination;
- author filtering without losing unrelated results or claiming completeness too early;
- selecting an article and parsing its full Markdown;
- parsing each confirmed statistics field, including missing/null values;
- non-success HTTP responses and API error envelopes;
- malformed or unexpected JSON;
- cancellation and timeouts;
- repeated refreshes and overlapping requests;
- UI state when there are no articles, partial results, or unavailable statistics;
- ensuring authentication values never enter logs or view models.

### Real-service acceptance test (manual, authorized)

With the user's own Personal API Key, verify:

1. The app authenticates without exposing the key.
2. The list matches the user's articles for the explicitly scanned pagination range.
3. A selected article's title and Markdown match the service.
4. Each displayed statistic matches the field returned by the service, with the source/time noted where useful.
5. Refresh updates the data and handles service errors without crashing the UI.
6. Selecting an article changes The's semantic focus, and TerminalAi can report the selected article identity and currently loaded facts without leaking credentials.

Record the exact commit, endpoint/fields verified, test conditions, and known coverage limits. Do not include credentials or private article contents in CI logs or committed fixtures.

## 8. Definition of done for the first real integration

- [ ] The Architect has a working article-library view backed by FSM_REST.
- [ ] Article list and full detail come from the actual CoderLegion read API.
- [ ] A bounded pagination strategy and its limitations are visible/documented.
- [ ] The statistics panel contains only confirmed API metrics and preserves unknown/missing values.
- [ ] Selection participates in the existing semantic focus / TerminalAi context path.
- [ ] Automated tests pass without credentials or network access.
- [ ] A manual authorized smoke test is recorded against an exact commit.
- [ ] No write/publish capability is accidentally reachable from the read-only feature.
- [ ] Documentation and Gallery explain what was actually verified, not what is merely planned.

## 9. Coordination protocol

Please have the LLM working in `TrentBest/The`:

1. Read this file and the FSM_REST sample links above.
2. Inspect The's existing service registration, settings/secret handling, MVVM conventions, diagnostic redaction, and TerminalAi focus/context pipeline before editing.
3. Implement the integration in The on its active development branch according to that repository's workflow; do not edit FSM_REST internals to accommodate WPF-specific concerns.
4. Report back with the files changed, the endpoint/field evidence for statistics, tests run, exact commit SHA, and remaining gaps.
5. If the API contract is insufficient to identify statistics endpoints, stop at the verified read path and list the precise missing contract facts rather than guessing.

This brief does not authorize merging to `master`, publishing packages, or deleting branches. Keep the work reviewable and preserve branch history.

## 10. Source-of-truth links

- The Architect repository: https://github.com/TrentBest/The
- FSM_REST development branch: https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/tree/development
- FSM_REST Gallery: https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/Gallery.md
- CoderLegion sample guide: https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/samples/CoderLegionArticleLibrary/README.md
- FSM_REST architecture contract: https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/docs/ARCHITECTURE.md
