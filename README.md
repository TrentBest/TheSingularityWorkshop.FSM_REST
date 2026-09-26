# TheSingularityWorkshop.FSM_REST

**REST transport boundary for the FSM ecosystem.**

FSM_REST defines the boundary through which an FSM-backed runtime can communicate with WebPage, AnyApp, remote clients, and future dedicated hosting infrastructure.

## Purpose

FSM_REST is a **transport boundary**, not a persistence engine, domain model, or MicroBundle implementation.

~~~text
Client / WebPage / AnyApp
        |
        v
     FSM_REST
        |
        v
 FSM ecosystem
   |    |    |
 Memory Serialization MicroBundles
        |
        v
      FSM_COS
        |
        v
   Experience
~~~

REST answers **how a runtime communicates across a process or network boundary**.

It does not define what an Experience means, how a MicroBundle implements its domain, where durable state is stored, or how a GUI manifests a capability.

## Hosting theory

The first implementation may run as an ordinary ASP.NET Core host and may be hosted by Azure or another web-capable environment.

That host is replaceable. A later migration to dedicated infrastructure centered around FSM_COS should change where communication is handled, not what an Experience or MicroBundle means.

## Design principles

### Transport is not semantics

REST carries identifiers, manifests, serialized representations, commands, and results. It should not need to understand the domain semantics of the objects it transports.

### WebPage is a proving ground

WebPage can exercise this boundary while the ecosystem is developed. FSM_REST must remain usable by other clients and hosts.

### Local execution remains possible

A client may execute an Experience locally through AnyApp or another runtime. REST should support remote retrieval and synchronization without making network execution mandatory.

### Hosting is replaceable

Azure, a WebPage-hosted service, and future dedicated FSM_COS infrastructure are hosting choices. They should expose the same conceptual runtime boundary.

## Current status

This repository is presently a minimal ASP.NET Core host. The HTTP surface is intentionally small while ecosystem contracts are established.

The first useful vertical slice is:

~~~text
retrieve -> manifest -> interact -> mutate -> persist
~~~

without embedding domain-specific logic here.

## Packaging

FSM_REST is currently a **host/service project**, not a NuGet package. A package becomes appropriate when reusable REST contracts, clients, or transport abstractions exist independently of the ASP.NET Core host.

## License

MIT. See LICENSE.txt.
