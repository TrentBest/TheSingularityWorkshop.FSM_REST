# FSM_REST Theory

## Form before implementation

FSM_REST exists to provide the reusable form through which REST capabilities can enter the FSM ecosystem.

It should not become the place where every REST technology is implemented.

This is the same composition principle used by MicroBundleDomain:

> the foundation describes and hosts composition; concrete capabilities arrive separately.

## Protocol descriptions are participants

OpenAPI is useful because it can describe REST APIs.

That does not make OpenAPI part of REST itself, nor does it make OpenAPI a permanent dependency of FSM_REST.

The intended architecture is:

~~~text
OpenAPI MicroBundle
       |
       v
 IProvider<OpenAPI>
       |
       v
   FSM_REST
       |
       v
 REST capability
~~~

The OpenAPI MicroBundle owns OpenAPI semantics.

FSM_REST owns the neutral REST surface.

This prevents the core package from accumulating protocol-specific assumptions.

## MicroBundles provide meaning

A MicroBundle can bring together:

- a description format;
- a provider;
- executable behavior;
- dependencies;
- optional GUI support;
- optional transport policy.

FSM_REST should consume those capabilities rather than trying to manufacture them internally.

## Transport principle

A distributed FSM ecosystem needs a boundary between semantic capability and communication.

IRestTransport is that boundary.

~~~text
capability
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

HTTP is one implementation of that boundary, not the definition of the boundary.

## GUI is downstream

FSM_REST should never decide that an operation is a button, form, card, table action, or workflow node.

It provides enough structured information for a downstream layer to make that decision.

~~~text
REST capability
      |
      +---- GUI manifestation
      |
      +---- FSM behavior
      |
      +---- serialization
      |
      +---- composition
      |
      v
  Experience
~~~

## Domain ownership remains external

A concrete service should not be baked into FSM_REST.

A concrete service can instead arrive as a REST MicroBundle whose implementation references the FSM_REST package.

That produces the dependency direction we want:

~~~text
FSM_REST
   ^
   |
REST MicroBundle
   ^
   |
host / experience
~~~

The substrate stays reusable while capabilities remain independently loadable.

## Current alpha boundary

Alpha 2 establishes:

- neutral REST capability descriptors;
- request and response forms;
- a transport abstraction;
- an HttpClient adapter;
- unit coverage around the boundary.

It intentionally does not establish an OpenAPI implementation.

The next architectural step is therefore **not** to add OpenAPI back into FSM_REST. It is to create the separate OpenAPI MicroBundle that consumes this substrate and exposes its own provider.

## Core invariant

**FSM_REST is the form. MicroBundles are the things composed through the form.**
