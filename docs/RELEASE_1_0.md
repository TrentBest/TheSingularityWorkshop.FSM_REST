# FSM_REST 1.0.0 Release Contract

## Purpose

FSM_REST 1.0.0 stabilizes the reusable REST capability surface without turning the package into an API-description monolith.

## Dependency boundary

```text
FSM_API
   │
FSM_COS 1.0.0
   │
FSM_REST 1.0.0
   ├── operation descriptors
   ├── runtime bindings
   ├── request construction
   └── transport boundary
```

FSM_REST does not own authentication policy, OpenAPI parsing, remote data, GUI generation, or application-specific REST services.

## What is stable

The release contract covers:

- REST API and operation descriptors;
- parameter and response metadata;
- reusable operation bindings;
- deterministic request construction;
- transport abstraction;
- HttpClient transport adapter;
- REST MicroBundle composition adapter;
- separation between capability description and runtime response data.

## What remains outside

The package does not own:

- authentication implementation;
- retry policy;
- caching policy;
- OpenAPI/YAML parsing;
- remote endpoint discovery;
- domain-specific schemas;
- GUI manifestation;
- Experience lifecycle.

Those concerns can be supplied by other packages and composed through the stable surface.

## The key theory

A REST MicroBundle describes **how a capability can be obtained**, not the changing dataset returned by that capability.

```text
capability recipe
      │
      ▼
operation descriptor
      │
      ▼
runtime binding
      │
      ▼
request
      │
      ▼
transport
      │
      ▼
current remote data
```

That distinction keeps reusable capability metadata separate from transient runtime state.

## Release review checklist

- [ ] FSM_COS 1.0.0 is available as a stable dependency.
- [ ] Release build succeeds.
- [ ] Complete test suite succeeds.
- [ ] Coverage collection succeeds.
- [ ] Public XML documentation produces no unexplained warnings.
- [ ] Packed nuspec contains no prerelease first-party dependency.
- [ ] README contains practical construction/execution examples.
- [ ] Theory explains the capability-versus-data boundary.
- [ ] Visuals accurately show the dependency and runtime boundaries.
- [ ] Publication remains manual and explicit.

> **Describe the capability once; obtain its changing result when the experience needs it.**
