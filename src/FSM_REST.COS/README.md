<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" />
  </a>
</p>

**Maintainer:** @TrentBest · [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)

# TheSingularityWorkshop.FSM_Rest.COS

## ✳️ Identity

FSM_COS integration for REST capabilities.

The base `TheSingularityWorkshop.FSM_Rest` package remains framework-agnostic: it owns REST descriptors, request construction, and transport.

This package is the optional composition boundary:

```text
FSM_REST → FSM_REST.COS → FSM_COS
```

`RestMicroBundle` carries a `RestApiDescriptor` into an FSM_COS composition as an executable MicroBundle capability.

The direction is intentional: REST does not need to know that FSM_COS exists. FSM_REST.COS is where that integration is requested.

## Package boundary

- Use **FSM_REST** for neutral REST capability descriptors, request construction, and transport.
- Use **FSM_REST.COS** when REST capability data must participate in FSM_COS composition.
- Keep domain behavior and provider-specific policy in the relevant host or domain package.

## Related resources

- [FSM_REST repository guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/README.md)
- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- [Source code](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/tree/development/src/FSM_REST.COS)
- [License](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/LICENSE.txt)


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
