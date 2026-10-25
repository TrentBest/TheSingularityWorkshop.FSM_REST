# CoderLegion Article Library — FSM_REST in a real workflow

This read-only console sample demonstrates FSM_REST against the CoderLegion API contract supplied in the project notes. It uses the real endpoint shapes `GET /posts?page=N` and `GET /posts/{id}`, builds each request from a `RestOperationDescriptor`, and sends it through `RestOperationExecutor` and `IRestTransport`.

## Requirements

- .NET 8 SDK
- A CoderLegion **Personal API Key** with access to the documented read endpoints
- Network access to `https://coderlegion.com/api/v1`

Do not use the Master API Key for this local workflow. Never commit the key, put it in a source file, pass it as a command-line argument, or print request headers.

## Configure

PowerShell:

```powershell
$env:CODERLEGION_API_KEY = "your-personal-api-key"
$env:CODERLEGION_HANDLE = "your-coderlegion-handle"
```

`CODERLEGION_HANDLE` is optional. When set, the listing command filters each returned page by `author.handle`. The endpoint documentation describes `GET /posts` as the paginated browse feed, not a dedicated “my posts” endpoint, so filtering happens client-side. Use a sufficiently high page limit to scan the relevant history; the sample intentionally has a configurable cap to prevent accidental unbounded requests.

## List articles

From the repository root:

```powershell
dotnet run --project samples/CoderLegionArticleLibrary -- list 100
```

Each page is fetched using `GET /posts?page=N`. Results include the post ID, title, author, date, and the short description supplied by the list endpoint. The current implementation stops when it reaches an empty `data` array or the specified page cap. **This is the sample's current stopping rule, not yet a verified CoderLegion pagination contract.** Until authoritative documentation confirms that an empty array definitively marks the end of the feed, an empty page must not be treated as proof that the user's full article archive was retrieved. If the page cap is reached, the scan is bounded and may be partial.

### Pagination contract limitation

Before relying on the listing as a complete archive, verify CoderLegion's authoritative pagination rules: page-number convention, empty-page semantics, and any documented total-count or next-page signal. Do not infer undocumented metadata. Until verified, report the result as a scan of the pages actually fetched, not a proven complete archive.

## Retrieve a complete article

```powershell
dotnet run --project samples/CoderLegionArticleLibrary -- show 101
```

The detail request uses `GET /posts/{id}` and prints the full `content` Markdown field. Replace `101` with an ID from your list.

## What this proves

- A concrete external API is described with FSM_REST operation metadata.
- Query and path parameters are bound at invocation time.
- The Personal API Key is injected at the host boundary through `X-API-Key`.
- HTTP status and JSON envelope are checked before the response is interpreted.
- The remote article body remains runtime data; the reusable operation description remains the capability recipe.

This sample does **not** create or edit posts, upload images, or publish anything. It does not claim a live request has succeeded until you run it with your own key. Write operations should be added as a separate, explicit step after the read-only path is verified.

## Next increment

After the read-only client works, extend it with a local draft file and a preview step. Then add `GET /posts/create-options` and an explicit user-confirmed `POST /posts` / `PUT /posts/{id}` workflow. Keep draft composition separate from the publish action, and never silently publish as a side effect of saving a local draft.
