# REST MicroBundles

## The useful trick

A REST API can be enormous at runtime while being small as a capability description.

That difference is exactly what a MicroBundle can exploit.

<p align="center">
  <img src="assets/fsm-rest-capability-recipe.svg" alt="REST MicroBundle recipe versus changing remote runtime data" width="900">
</p>

~~~text
REMOTE SERVICE
     │
     │ potentially enormous,
     │ dynamic, user-specific data
     ▼
┌──────────────────────┐
│      REST API        │
└──────────┬───────────┘
           │
           │ describe how to ask
           ▼
┌──────────────────────┐
│    REST MicroBundle  │
│ endpoint identity    │
│ method + path        │
│ parameters           │
│ request rules        │
│ response metadata    │
│ provider behavior    │
└──────────┬───────────┘
           │
           ▼
       RestRequest
           │
           ▼
     IRestTransport
           │
           ▼
      current data
~~~

The bundle does not need to contain the current response.

It contains the information required to obtain the response.

## Recipe versus result

| Stored in the capability | Produced at runtime |
|---|---|
| API identity | response body |
| operation identity | current records |
| HTTP method | current prices |
| path | current inventory |
| parameter definitions | user-specific results |
| request-body rules | server-generated values |
| response metadata | current timestamps |
| provider behavior | transient transport data |

> **Store the instructions for obtaining the data; obtain the data when the capability is used.**

## Minimal composition example

The REST package does not depend on MicroBundleDomain.

A separate provider can combine the two concepts:

~~~csharp
using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.MicroBundleDomain;

var bundle = new MicroBundleDescriptor(
    id: 0x2001UL,
    version: "1.0.0",
    providers:
    [
        new MicroBundleProvider("rest:catalog")
    ]);

var api = new RestApiDescriptor(
    "Store Catalog",
    "1.0",
    [
        new RestOperationDescriptor(
            "GET",
            "/products",
            "listProducts",
            "List products",
            "Returns the current product catalog.",
            [],
            null,
            [
                new RestResponseDescriptor(
                    "200",
                    "Current product collection.",
                    ["application/json"],
                    "array",
                    null)
            ])
    ]);
~~~

This is illustrative rather than a new FSM_REST dependency.

The separation is the important part:

~~~text
MicroBundleDomain
      │
      │ identifies the loadable capability
      ▼
REST provider
      │
      │ supplies REST semantics
      ▼
FSM_REST
      │
      │ supplies neutral REST forms
      ▼
FSM_COS / host
      │
      ▼
runtime execution
~~~

## From operation to request

The provider or experience binds runtime values and creates a request.

~~~text
RestOperationDescriptor
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

The descriptor remains reusable capability data.

The request represents one execution.

## Why not store the response?

Because the response is not the capability.

A product catalog may change every minute. User visibility may change between requests. Inventory may be authoritative somewhere else.

The MicroBundle should not need to change because remote state changed.

It describes the access path and the capability semantics.

## Where caching belongs

Caching can still exist.

It simply does not belong in the fundamental REST capability contract.

~~~text
capability
    │
    ▼
RestRequest
    │
    ▼
host policy
    │
    ├── cache?
    ├── retry?
    ├── auth?
    └── telemetry?
    │
    ▼
IRestTransport
    │
    ▼
remote API
~~~

A host may cache responses.

A provider may define cache semantics.

A warehouse may persist selected data.

FSM_REST does not need to own those decisions.

## Multiple APIs, one shape

Once REST capability is represented neutrally, multiple providers can participate in the same composition surface.

~~~text
OpenAPI provider ──────┐
hand-authored provider ┤
vendor adapter ────────┤
other description ─────┘
                       │
                       ▼
                  FSM_REST
                       │
              ┌────────┼────────┐
              ▼        ▼        ▼
             GUI      FSM     storage
~~~

The source changes.

The downstream composition surface does not.

<p align="center">
  <img src="assets/fsm-rest-composition-map.svg" alt="REST providers converging on the neutral FSM_REST composition surface" width="900">
</p>

## Storefront implications

A storefront can be a composition of capabilities rather than a hard-coded collection of pages.

~~~text
                    STOREFRONT
                         │
          ┌──────────────┼──────────────┐
          ▼              ▼              ▼
       catalog         orders        identity
          │              │              │
          ▼              ▼              ▼
      REST bundle     REST bundle    REST bundle
          │              │              │
          └──────────────┼──────────────┘
                         ▼
                      FSM_COS
                         │
                         ▼
                       GUI
~~~

The storefront owns the experience.

The MicroBundles own the capabilities.

The remote services remain remote.

FSM_REST provides the common REST vocabulary connecting those layers.

## Boundary

A REST MicroBundle is not automatically:

- a credential store;
- a cache;
- a billing system;
- an OpenAPI implementation;
- a GUI;
- a domain model;
- an API marketplace.

Those can be composed around it.

The point is to make the REST edge cheap and reusable enough that higher-level systems have a clean thing to build upon.

## Related documents

- [README](../README.md)
- [REST Capability Model](REFLECTION.md)
- [FSM_REST Theory](THEORY.md)
