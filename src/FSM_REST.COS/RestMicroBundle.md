# RestMicroBundle

**Workshop moniker:** @TrentBest  
**Purpose:** Carry a `RestApiDescriptor` through the MicroBundleDomain contract for optional FSM_COS composition.

## Responsibilities
- Implement `IMicroBundle` and expose the wrapped REST API descriptor.
- Derive the default bundle version from the API descriptor version.
- Create a default provider identity using `rest:{api.Title}` when none is supplied.
- Project declared bundle dependencies into unconfigured dependency requests.
- Validate non-null load and arbitration contexts; the current load hook performs no extra work and arbitration returns false.

## Boundaries
This is an integration adapter, not a REST transport, API executor, or domain-specific endpoint implementation. Its presence is optional for core REST consumers.

## Source and verification
- Source: [RestMicroBundle.cs](RestMicroBundle.cs)
- Related helper: [RestApiDescriptorExtensions](RestApiDescriptorExtensions.md)
- UML: [Core architecture](../../docs/UML/FSM_REST.Core.UML.md)
- Tests: [FSM_REST tests](../../tests/FSM_REST.Tests/)
- Provenance: manually authored companion; confirm the MicroBundle contract against the referenced package version.
