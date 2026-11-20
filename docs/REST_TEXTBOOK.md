<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# REST Systems: A Living Textbook

**Maintainer:** @TrentBest

This textbook teaches REST as an architectural boundary—not merely a collection of HTTP verbs—and explains how FSM_REST models that boundary as composable capabilities. Chapters are deliberately independent so instructors can reorder them, add local examples, or replace an exercise without rewriting the whole book.

> **Teaching principle:** explain the human problem, formalize the idea, show a living visual, connect the theory to code, and give readers a way to challenge or verify the claim.

## Learning path

1. [Chapter 1 — From resources to representations](theory/Chapter-01-Resources-Representations.md): resource identity, representations, identifiers, and why REST is not simply “JSON over HTTP.”
2. [Chapter 2 — The network is a boundary](theory/Chapter-02-The-Network-Boundary.md): latency, partial failure, timeouts, cancellation, retries, idempotency, and uncertainty.
3. [Chapter 3 — Capability, recipe, execution, result](theory/Chapter-03-Capability-Recipe-Execution.md): how FSM_REST separates reusable capability description from per-call inputs, transport, and changing remote data.
4. [Chapter 4 — Promises, observations, and evidence](theory/Chapter-04-Promises-Observations-Evidence.md): how to distinguish intended requests, observed outcomes, domain validation, and safe reconciliation.

## Instructor-friendly structure

- **Lecture note:** add a local Instructor note section to the relevant chapter.
- **Case study:** add a provider-neutral example and state which assumptions are provider-specific.
- **Assessment:** place questions and answer keys in separate files.
- **Lab:** include runnable sample, expected output, prerequisites, and a verification command.
- **Visual:** label each SVG conceptual, source-anchor-checked, or captured from a running example.

The chapters are conceptual teaching material unless code is explicitly marked verified. The illustrations are authored teaching models, not captures of a live server or proof of runtime behavior. This follows the evidence discipline used in FSM_COS: conceptual animation, authored diagrams with explicit source-anchor checks, and live runtime observations are distinct provenance levels.

## Living illustrations

- [Resource, representation, and request flow](assets/rest-textbook-resource-flow.svg)
- [Failure and uncertainty at the network boundary](assets/rest-textbook-network-boundary.svg)
- [FSM_REST capability lifecycle](assets/rest-textbook-capability-lifecycle.svg)
- [The REST evidence boundary](assets/rest-textbook-evidence-boundary.svg)
- [FSM_REST core contract surface (source-anchor-checked)](assets/rest-contract-surface.svg)

These SVGs are editable, versionable, scalable assets that evolve with the lessons. Their animation explains a concept; it is not live runtime telemetry.

### Visual provenance: three levels

FSM_COS's living-SVG work makes an important distinction that this textbook adopts:

| Level | Meaning | Evidence required |
|---|---|---|
| **Conceptual** | An authored visual teaches a model or relationship | The visual labels itself conceptual and does not imply measured execution |
| **Source-anchor-checked** | An authored diagram names source symbols or members that a deterministic checker verifies exist; the diagram's relationships are not automatically inferred | The checker names its anchors and runs in CI; claims beyond those checks remain authored and must be reviewed |
| **Runtime-observed** | A visual presents a real execution or measurement | The displayed state comes from a defined, validated runtime observation channel |

The four animated textbook SVGs are **conceptual**. Their animation is explanatory motion, not live telemetry. The core contract surface diagram is an **authored, source-anchor-checked** view: `scripts/generate_rest_contract_diagram.py` verifies that named contract anchors exist in the current C# source and checks that the committed SVG matches its deterministic template. CI runs it in `--check` mode and fails on missing/changed anchors or diagram drift. The checker does not parse C# or infer the diagram's edges; those relationships remain authored documentation and should be reviewed against source. This is not a general C# parser, reflection dump, or GUI.SVG output. A future runtime-observed view would require a separate, validated observation source.

## How this textbook relates to the package

- [Package theory](THEORY.md) — concise architectural principles.
- [Consumer guide](CONSUMING.md) — how to use the package.
- [Architecture contract](ARCHITECTURE.md) — implementation boundaries.
- [Runnable capability demo](../samples/RestCapabilityDemo/Program.md) — deterministic example.
- [UML index](UML/README.md) — source-grounded type and interaction diagrams.

A descriptor cannot prove a remote service exists, is reachable, is secure, returns valid data, or assigns the same meaning to a response that your domain does. Those are separate contracts to establish and verify.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
