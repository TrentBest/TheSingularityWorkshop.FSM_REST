# FSM_REST 1.0.0 Release Contract

## Purpose

FSM_REST 1.0.0 stabilizes the reusable REST capability surface without turning the package into an API-description monolith.

## Dependency boundary

FSM_REST is a foundation package, not a downstream composition package.

~~~text
FSM_REST 1.0.0
   ├── operation descriptors
   ├── runtime bindings
   ├── request construction
   └── transport boundary

          ▲
          │ consumed by
          │
     FSM_COS / other composition layers
~~~

There is intentionally **no dependency on FSM_COS**.

If FSM_COS needs to represent a REST capability as an `IMicroBundle`, FSM_COS consumes FSM_REST and owns that adapter.

## What is stable

The release contract covers:

- REST API and operation descriptors;
- parameter and response metadata;
- reusable operation bindings;
- deterministic request construction;
- transport abstraction;
- HttpClient transport adapter;
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

- [ ] Package restores without FSM_COS or any other Workshop dependency.
- [ ] Release build succeeds.
- [ ] Complete test suite succeeds.
- [ ] Coverage collection succeeds.
- [ ] Public XML documentation produces no unexplained warnings.
- [ ] Packed nuspec contains no prerelease first-party dependency.
- [ ] README contains practical construction/execution examples.
- [ ] Theory explains the capability-versus-data boundary.
- [ ] Visuals accurately show the independent REST foundation and downstream composition boundary.
- [ ] Publication remains manual and explicit.

> **Describe the capability once; obtain its changing result when the experience needs it.**
