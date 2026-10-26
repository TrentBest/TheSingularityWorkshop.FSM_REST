<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# Storefront Composition: Walmart + Fred Meyer

This sample proves the **same normalized shopping workflow over two provider adapters**. The default executable uses deterministic responses; the opt-in Fred Meyer mode makes real read-only API calls. It lives beside FSM_REST because FSM_REST owns the neutral request/transport boundary; the storefront experience owns shopping semantics.

## What this proves

- One provider-neutral product shape and shopping workflow.
- Provider-specific endpoint and authentication details stay inside adapters.
- The same IRestTransport / RestRequest boundary can send requests and can be replaced by a deterministic test transport.
- Provider limitations are represented honestly rather than pretending all stores expose the same API.

## Purchase-history insight proof

The deterministic run also prints a clearly labelled, fictional purchase-history comparison: a previous 14.5 oz purchase for $2.49 versus an illustrative 32 oz offer for $4.49. These are fixture values, not real purchases, scraped prices, or live offers. The calculation reports historical/current price per unit, the difference and percentage change, and the upfront package-price difference.

A lower unit price is calculable when product identity and units are comparable. “You would not have needed to buy again yet” is a separate, uncertain estimate that requires a consumption model grounded in the shopper’s own history. The sample deliberately does not claim that from unit pricing alone. The arithmetic and input checks live in `PurchaseInsights.cs`; persistent history, product matching, and polished UI are future work tracked in [issue #9](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/issues/9).

## Important API reality

These are not equivalent public APIs:

- **Walmart shopper catalog / pickup-and-delivery (OPD):** Walmart documents localized product, price, availability, store-locator and “Add To Cart” capabilities for approved partners. Access requires partner/affiliate onboarding; it is not an unrestricted public consumer checkout API. See [Walmart OPD introduction](https://www.walmart.io/docs/opd/stores/grocery-introduction).
- **Walmart Marketplace APIs:** These are seller/solution-provider APIs for managing a seller's own listings, inventory and orders. They are not a consumer grocery-shopping API. See [Walmart Marketplace API overview](https://developer.walmart.com/us-marketplace/docs/integrate-with-marketplace-apis).
- **Fred Meyer:** Fred Meyer is served through Kroger's public API platform. Public API onboarding requires a registered application and OAuth credentials; the public flow supports product/location discovery and cart actions, but the public APIs do not provide a general consumer checkout/order-placement endpoint. See [Kroger Public APIs](https://www.postman.com/kroger/the-kroger-co-s-public-workspace/collection/ki6utqb/kroger-public-apis).

Therefore, this sample does not claim to place a real consumer order. It demonstrates a safe local order preview; checkout handoff is intentionally not automated yet. Real order submission must only be added if the relevant provider grants an explicit supported API for that account and use case. Walmart Marketplace sandbox order simulation is a separate seller workflow, not Walmart grocery checkout.

## Proof stages

1. **Deterministic composition proof:** From this directory, run `dotnet run`. Both adapters use a fake IRestTransport, return normalized products and prepare local cart previews. This makes no network calls.
2. **Fred Meyer live catalog:** Register an app at the [Kroger Developer Portal](https://developer.kroger.com/), set `KROGER_CLIENT_ID` and `KROGER_CLIENT_SECRET`, then run `dotnet run -- --live-fred-meyer "milk"`. The sample uses OAuth client credentials and queries the product API through FSM_REST. It is read-only; product search is not a guarantee of shelf quantity or checkout availability.
3. **Walmart live catalog / availability:** Walmart's shopper OPD APIs require approved partner access and issued credentials/configuration. Once access is approved, implement the exact contracted endpoint set in a dedicated adapter. Do not substitute Marketplace seller endpoints.
4. **Checkout handoff:** Open the provider's own cart/checkout experience after the user reviews the cart. Do not automate payment or submit a live order from a sample.

## Deliberate safety boundaries

- No secrets in source control.
- No checkout, payment, or real order submission from the demo.
- No assertion that product search equals in-store shelf inventory.
- No real API calls in the default run.
- A local order preview is not a provider-confirmed order.

## Official references

- [Walmart OPD API introduction](https://www.walmart.io/docs/opd/stores/grocery-introduction)
- [Walmart Marketplace API overview](https://developer.walmart.com/us-marketplace/docs/integrate-with-marketplace-apis)
- [Kroger Public APIs onboarding](https://www.postman.com/kroger/the-kroger-co-s-public-workspace/collection/ki6utqb/kroger-public-apis)
- [FSM_REST README and capability recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/README.md)

---

<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>Forging Software for the Singularity.</em><br />
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST">FSM_REST repository</a>
</p>
