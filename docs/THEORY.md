# FSM_REST Theory

## The transport principle

A distributed FSM ecosystem needs a boundary between semantic execution and communication.

FSM_REST occupies that boundary.

It should be possible to replace browser communication with a desktop client, WebPage hosting with a dedicated server, or HTTP with another transport adapter without redefining the Experience model.

## Reflection before execution

FSM_REST has two related but distinct responsibilities:

1. describe the capabilities exposed by a REST service;
2. provide the transport boundary through which those capabilities may later be invoked.

The first responsibility is reflection.

~~~text
REST description
      |
      v
    reflect
      |
      v
capability descriptor
      |
      v
GUI manifestation
      |
      v
behavior assignment
      |
      v
execute
~~~

Reflection must not require execution.

This allows a GUI to inspect an API before a request is made and allows the same reflected capability to be manifested differently in different experiences.

## The Postman-like model

Traditional API tooling presents a human with an API surface and asks the human to construct requests.

FSM_REST can take the next conceptual step:

~~~text
API URL
  |
  v
discover description
  |
  v
reflect API
  |
  v
generate capability palette
  |
  v
assign GUI behavior
  |
  v
execute capability
~~~

The goal is not to reproduce a conventional API client.

The goal is to turn an external API into material from which an Experience can be composed.

A REST endpoint can therefore become a GUI capability without the GUI author writing a bespoke integration for that endpoint.

## Request and response are observations

A REST request is an observation or request against an FSM ecosystem, not a definition of the ecosystem itself.

~~~text
Request
  -> identify
  -> retrieve
  -> resolve
  -> execute
  -> mutate
  -> serialize
  -> respond
~~~

The transport layer coordinates these steps but does not own their meaning.

## Stateless transport, persistent reality

HTTP requests are transient. Experiences are not required to be.

- transport state belongs to the communication mechanism;
- runtime state belongs to FSM execution;
- persistent state belongs to FSM_Memory;
- serialized representation belongs to FSM_Serialization;
- domain meaning belongs to MicroBundle/domain packages.

A request may therefore be short-lived while the digital reality it addresses persists.

## Identity before interpretation

The transport layer should prefer stable identifiers and opaque payloads over domain-specific assumptions.

For reflected APIs, operationId is preferred when the API supplies it. Path and HTTP method provide a deterministic fallback when it does not.

The reflector should describe what exists without pretending to understand why the remote service exists.

## GUI is a downstream manifestation

FSM_REST should never decide that an operation is a button, form, card, table action, or workflow node.

It should provide enough structured information for another layer to make that decision.

~~~text
RestOperationDescriptor
        |
        +-- method
        +-- path
        +-- identity
        +-- parameters
        +-- request body
        |
        v
       GUI
~~~

This is what makes the reflection model reusable.

## Remote execution is an option

The architecture permits:

1. local execution with remote persistence;
2. local execution with remote retrieval and synchronization;
3. remote execution with the client acting primarily as a manifestation surface.

The semantic model remains the same in all three arrangements.

## Migration invariant

A successful infrastructure migration preserves:

~~~text
Identity
Composition
Version
Lineage
State
Behavior
~~~

while allowing the host, transport implementation, storage implementation, and execution placement to change.

That is the central hosting invariant for the FSM ecosystem.

## Current alpha boundary

The first alpha intentionally reflects OpenAPI 3.x JSON.

It does not yet attempt to solve:

- YAML parsing;
- reference resolution;
- complete schema reflection;
- authentication configuration;
- URL discovery;
- arbitrary HTTP execution;
- automatic GUI generation.

Those capabilities can be layered on without changing the central distinction:

**reflection describes a capability; execution performs a capability; GUI manifests a capability.**
