<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS / MicroBundleDomain Integration UML

**Scope:** Optional composition adapter. This integration is not a dependency of FSM_REST core; core execution depends directly on FSM_API.

## Target diagrams

- [RestMicroBundle](../RestMicroBundle.UML.md) — MicroBundleDomain-facing adapter.
- [RestApiDescriptorExtensions](../RestApiDescriptorExtensions.UML.md) — descriptor conversion helpers for the adapter.

## Source companions

- [RestMicroBundle source companion](../../../src/FSM_REST.COS/RestMicroBundle.md)
- [RestApiDescriptorExtensions source companion](../../../src/FSM_REST.COS/RestApiDescriptorExtensions.md)
- [FSM_REST.COS package overview](../../../src/FSM_REST.COS/README.md)

## Use case

A composition host can adapt REST API descriptors into its MicroBundleDomain workflow through the optional package. This is an integration boundary, not a reason for the core package to reference FSM_COS or MicroBundleDomain.

Return to the [complete UML index](../README.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
