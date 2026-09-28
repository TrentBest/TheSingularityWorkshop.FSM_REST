# REST Reflection Model

FSM_REST treats a REST API description as a source of capabilities.

The reflection layer converts protocol-oriented metadata into a framework-agnostic model that another system can manifest.

## Pipeline

~~~text
OpenAPI document
      |
      v
OpenApiRestReflector
      |
      v
RestApiDescriptor
      |
      +---- RestOperationDescriptor
      |          |
      |          +---- parameters
      |          +---- request body
      |
      v
GUI capability palette
~~~

The reflector does not execute requests. That is deliberate.

Reflection answers:

> What can this API do?

Transport answers:

> How do we communicate with it?

GUI answers:

> How should that capability appear to a person?

FSM behavior can eventually answer:

> What should happen when that capability is invoked?

## Why descriptors?

A GUI should not have to understand OpenAPI directly.

For example, an operation such as:

~~~text
POST /users
operationId = createUser
request body = application/json
~~~

can become a REST operation descriptor.

The GUI can then decide whether that descriptor becomes a button, form, card, table action, node in a visual workflow, or another GUI primitive.

The REST layer has no opinion about that visual representation.

## Stable identity

OperationId is preferred when supplied by the API description. When it is absent, FSM_REST currently creates a deterministic fallback from HTTP method and path.

This is an alpha behavior and may become more formalized as the serialization and GUI layers mature.

## Parameters

The alpha reflector recognizes the OpenAPI parameter locations:

- path
- query
- header
- cookie

Path-level parameters and operation-level parameters are both considered. Operation-level definitions override a path-level definition with the same location/name identity.

## Request bodies

The alpha reflector records:

- whether the request body is required;
- supported media types;
- basic schema type;
- basic schema format.

Full schema reflection is intentionally deferred.

## Deliberate boundary

FSM_REST does not:

- interpret business meaning;
- execute arbitrary remote URLs;
- resolve credentials;
- persist API state;
- generate GUI controls;
- create FSM transitions automatically.

Those are separate concerns.

The eventual system can compose them:

~~~text
REST reflection
      +
GUI manifestation
      +
FSM behavior
      +
FSM serialization
      +
FSM_COS composition
      =
dynamic application surface
~~~

## Next reflection increments

1. response-schema reflection;
2. reusable schema descriptors;
3. OpenAPI reference resolution;
4. authentication/security descriptors;
5. safe URL-based discovery;
6. a REST execution abstraction;
7. GUI palette projection.

Each increment should preserve the separation between describing a capability and executing it.
