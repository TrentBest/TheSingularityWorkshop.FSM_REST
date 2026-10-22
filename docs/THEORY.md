# FSM_REST Theory

## Form before implementation

FSM_REST provides the reusable form through which REST capabilities enter the FSM ecosystem.

It should not become the place where every REST technology is implemented.

> **FSM_REST describes the shape of a REST capability. A MicroBundle supplies the particular capability.**

## Capability data is not remote data

The important distinction is between the **recipe** for obtaining a capability and the **result** produced by using it.

~~~text
CAPABILITY RECIPE
      │
      ▼
RestOperationDescriptor
      │
      ▼
RestRequest
      │
      ▼
IRestTransport
      │
      ▼
REMOTE RESPONSE
      │
      ▼
runtime data
~~~

The descriptor belongs to composition.

The response belongs to execution.

A MicroBundle can therefore remain stable while the remote service changes its data.

## Protocol descriptions are participants

OpenAPI is useful because it describes REST APIs.

That does not make OpenAPI a permanent dependency of FSM_REST.

~~~text
OpenAPI MicroBundle
       │
       ▼
 provider / adapter
       │
       ▼
   FSM_REST
       │
       ▼
 REST capability
~~~

The OpenAPI MicroBundle owns OpenAPI semantics.

FSM_REST owns the neutral REST surface.

This keeps protocol-specific assumptions out of the substrate.

## MicroBundles provide meaning

A MicroBundle can bring together:

- description format;
- provider;
- executable behavior;
- dependencies;
- optional GUI support;
- optional transport policy;
- REST capability descriptors.

FSM_REST consumes those capabilities rather than manufacturing concrete services internally.

~~~text
MicroBundle
    │
    ├── description
    ├── provider
    ├── behavior
    └── REST capability
              │
              ▼
          FSM_REST
~~~

## Transport principle

IRestTransport is the boundary between semantic capability and communication.

~~~text
capability
    │
    ▼
RestRequest
    │
    ▼
IRestTransport
    │
    ▼
RestResponse
~~~

HTTP is one implementation of that boundary, not the definition of it.

HttpClientRestTransport is therefore an adapter. A host can replace it without changing capability descriptors.

## GUI is downstream

FSM_REST does not decide that an operation is a button, form, card, table action, or workflow node.

It provides structured capability data for downstream layers.

~~~text
REST capability
      │
      ├──── GUI manifestation
      ├──── FSM behavior
      ├──── serialization
      └──── composition
                 │
                 ▼
             Experience
~~~

## Cheap by construction

A REST capability can be large in behavior while remaining small in description.

The MicroBundle needs the information required to construct and understand requests. It does not need every response the remote service can ever produce.

~~~text
SMALL DESCRIPTION
      │
      │ method / path / parameters /
      │ request rules / response metadata
      ▼
REMOTE CAPABILITY
      │
      │ current state
      ▼
LARGE / DYNAMIC RESULT
~~~

This is not a claim that every REST integration is physically small.

It is a composition property:

> **The bundle describes the access path and capability semantics instead of duplicating the remote dataset.**

That makes external capabilities attractive candidates for composable Workshop content.

## Domain ownership remains external

A concrete service should not be baked into FSM_REST.

A concrete service can arrive as a REST MicroBundle whose implementation references FSM_REST. If that bundle needs FSM_COS composition, the optional `TheSingularityWorkshop.FSM_Rest.COS` package owns that upward integration.

~~~text
FSM_REST
   ▲
   │
REST MicroBundle
   ▲
   │
host / experience
~~~

The substrate stays reusable while capabilities remain independently loadable.

## Current alpha boundary

Alpha 4 establishes:

- neutral REST capability descriptors;
- stable operation presentation metadata;
- request and response forms;
- response success classification;
- a transport abstraction;
- an HttpClient adapter;
- unit coverage around the boundary;
- documented MicroBundle composition guidance;
- separation of the REST transport substrate from optional FSM_COS integration;
- a separate REST-to-FSM_COS adapter package so the core REST package remains independent of FSM_COS.

It intentionally does not establish:

- OpenAPI implementation;
- YAML implementation;
- credential store;
- GUI renderer;
- response warehouse;
- concrete remote services.

Those remain separately composable.

## Package dependency direction

The dependency direction is intentional:

```text
TheSingularityWorkshop.FSM_Rest
        │
        │ neutral REST capability + transport
        ▼
provider / host

TheSingularityWorkshop.FSM_Rest.COS
        │
        │ optional composition adapter
        ▼
FSM_COS
```

The core REST package does **not** depend on FSM_COS. A host that wants REST capabilities to become FSM_COS MicroBundles can add the separate `.COS` package.

This preserves the Workshop rule that lower-level capability packages do not depend upward on the composition kernel.

## Core invariant

**FSM_REST is the form. MicroBundles are the things composed through the form. FSM_REST.COS is the optional bridge into FSM_COS.**

The response is runtime data.

The bundle is the capability recipe.


## Execution contract: what each stage guarantees

The boundary is useful only when each stage has a narrow, predictable responsibility:

| Stage | It guarantees | It does not guarantee |
|---|---|---|
| Descriptor | Reusable operation metadata and parameter/response descriptions | That a remote endpoint exists or follows the description |
| Binding | A snapshot of runtime parameter values for one invocation | Authentication, authorization, or validation against a remote schema |
| Request factory | A concrete URI, method, headers, and optional body from the supported inputs | Network I/O, retries, caching, or domain validation |
| Transport | Sends the constructed request, observes the response, and propagates cancellation | That a response is successful, safe, trustworthy, or meaningful to the caller |
| Consumer | Interprets the response under its domain contract | Nothing is inferred automatically from HTTP success alone |

A non-success HTTP status is still a response, not automatically an exception. The caller must decide whether to retry, report, transform, or reject it. Cancellation and transport failures must also be handled by the caller's execution policy.

## Parameter and schema fidelity

The core parameter model is intentionally shallow. Runtime values are strings; path and query values are percent-encoded, declared header values are copied to request headers, and cookie values are encoded into a Cookie header. The factory does not implement the full set of OpenAPI parameter serialization rules for arrays, objects, style, or explode, and it does not validate a request or response body against a schema. A provider translating a richer external description must preserve that richer meaning in its own adapter or reject unsupported forms explicitly rather than silently claiming equivalence.

The same applies to response metadata: media types and basic schema type/format describe a response; they do not deserialize or validate its body. RestResponse.Body remains transport output until a downstream consumer interprets it.

## The practical design rule

Use FSM_REST when you want to model a reusable REST operation and execute one request through a replaceable communication boundary. Add a provider when you need a source format, an adapter when you need a particular transport policy, and a host/domain layer when you need authentication, authorization, validation, retries, persistence, or business meaning. Keep those decisions explicit so the small core remains dependable and independently consumable.
