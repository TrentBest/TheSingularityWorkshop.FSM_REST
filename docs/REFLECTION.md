# REST Capability Model

FSM_REST describes the **shape of a REST capability** without owning the format that originally described it.

## From description to capability

~~~text
┌───────────────────────────┐
│ description MicroBundle   │
│ OpenAPI / custom / other  │
└─────────────┬─────────────┘
              │
              ▼
       provider / adapter
              │
              ▼
      RestApiDescriptor
              │
              ├── RestOperationDescriptor
              │       ├── parameters
              │       ├── request body
              │       └── responses
              │
              ▼
        GUI / FSM / Experience
~~~

The description format is external. OpenAPI is one possible participant.

## Descriptor semantics

### RestApiDescriptor

Describes a collection of REST operations.

### RestOperationDescriptor

Describes one REST operation using:

- HTTP method;
- path;
- stable operation identity;
- summary and description;
- parameter metadata;
- request-body metadata;
- response metadata.

Its PaletteLabel gives downstream tooling a useful human-readable label without making GUI a dependency.

### Parameters

The neutral parameter model supports locations such as path, query, header, and cookie.

The source format translates its own semantics into this representation.

### Request and response bodies

RestRequestBodyDescriptor records basic request-body information.

RestResponseDescriptor records:

- status-code identity;
- description;
- media types;
- basic schema type;
- basic schema format.

These are deliberately shallow contracts. A protocol MicroBundle may carry richer schemas without forcing that model into FSM_REST.

## Capability recipe versus execution

The descriptor describes the operation.

The request represents one execution.

~~~text
reusable capability data
          │
          ▼
RestOperationDescriptor
          │
   bind runtime values
          │
          ▼
     RestRequest
          │
          ▼
    IRestTransport
          │
          ▼
      RestResponse
          │
          ▼
      current data
~~~

A MicroBundle can store reusable capability description without storing changing response payloads.

<p align="center">
  <img src="assets/fsm-rest-execution-flow.svg" alt="REST capability execution flow from reusable operation through request construction and transport" width="900">
</p>

## Stable identity

OperationId is the preferred identity when a source provides one.

When no source-defined identity exists, a provider can derive a deterministic identity from method and path.

FSM_REST does not require that identity to come from OpenAPI.

## Transport is separate

Provider work answers:

> **What capability exists?**

Transport answers:

> **How do we communicate with it?**

IRestTransport makes that distinction explicit.

## GUI is downstream

FSM_REST does not decide that an operation is a button, form, card, table action, or workflow node.

It provides structured capability data from which another layer can make that decision.

## Deliberate non-responsibilities

FSM_REST does not:

- parse OpenAPI;
- parse YAML;
- resolve format-specific references;
- discover credentials;
- generate GUI controls;
- define concrete remote services;
- own domain-specific MicroBundles;
- persist response payloads.

Those concerns belong in separately composable packages or host policy.

## Composition invariant

~~~text
format/domain package
        │
        ▼
provider
        │
        ▼
FSM_REST
        │
        ├──── transport
        ├──── capability model
        │
        ▼
FSM_COS / GUI / FSM_API
        │
        ▼
Experience
~~~

The REST package is a **form**, not a warehouse of implementations.

The visual model is deliberate: the reusable description remains on one side of the boundary; runtime values and remote state cross the boundary only when the capability is executed.
