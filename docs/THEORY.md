# FSM_REST Theory

## The transport principle

A distributed FSM ecosystem needs a boundary between semantic execution and communication.

FSM_REST occupies that boundary.

It should be possible to replace browser communication with a desktop client, WebPage hosting with a dedicated server, or HTTP with another transport adapter without redefining the Experience model.

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

A future request may identify an Experience and ask for its current representation. FSM_REST does not need to know whether that Experience is a workshop, game, laboratory, world, or something entirely new.

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
