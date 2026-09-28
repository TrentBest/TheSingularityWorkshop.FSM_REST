# REST Capability Model

FSM_REST describes the **shape of a REST capability** without claiming ownership of the format that originally described it.

That distinction is fundamental.

## The boundary

~~~text
description MicroBundle
        |
        v
 provider / adapter
        |
        v
 RestApiDescriptor
        |
        +---- RestOperationDescriptor
        |          |
        |          +---- parameters
        |          +---- request body
        |          +---- responses
        |
        v
 GUI / FSM / Experience
~~~

The description format is external.

OpenAPI is one possible MicroBundle. Another format could provide the same REST capability surface without requiring FSM_REST to know that format exists.

## Descriptor semantics

RestApiDescriptor describes a collection of REST operations.

RestOperationDescriptor identifies an operation using:

- HTTP method;
- path;
- operation identity;
- optional summary and description;
- parameter metadata;
- request-body metadata;
- response metadata.

The descriptor does not execute anything.

## Parameters

The neutral parameter model supports:

- path;
- query;
- header;
- cookie.

The source format is responsible for translating its own parameter semantics into this neutral representation.

## Request and response bodies

RestRequestBodyDescriptor records basic request-body information.

RestResponseDescriptor records:

- status-code identity;
- description;
- media types;
- basic schema type;
- basic schema format.

These are deliberately shallow contracts. A protocol MicroBundle may carry richer schemas without forcing that schema model into FSM_REST.

## Stable identity

OperationId is the preferred identity when a source format provides one.

When no source-defined identity exists, a provider can derive a deterministic identity from the method and path.

FSM_REST does not require that identity to come from OpenAPI.

## Transport is separate

Provider work answers:

> What capability exists?

Transport answers:

> How do we communicate with it?

IRestTransport makes that distinction explicit.

~~~text
provider
   |
   v
REST capability
   |
   v
RestRequest
   |
   v
IRestTransport
   |
   v
RestResponse
~~~

A host can replace HttpClientRestTransport with another adapter without changing the capability descriptors.

## Deliberate non-responsibilities

FSM_REST does not:

- parse OpenAPI;
- parse YAML;
- resolve format-specific references;
- discover credentials;
- generate GUI controls;
- define concrete remote services;
- own domain-specific MicroBundles.

Those concerns belong in separately composable packages.

## Composition invariant

The architecture should remain:

~~~text
format/domain package
        |
        v
provider
        |
        v
FSM_REST
        |
        +---- transport
        +---- capability model
        |
        v
FSM_COS / GUI / FSM_API
~~~

The REST package is therefore a **form**, not a warehouse of implementations.
