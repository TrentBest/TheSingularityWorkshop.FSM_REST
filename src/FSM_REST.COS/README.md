![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# TheSingularityWorkshop.FSM_Rest.COS

FSM_COS integration for REST capabilities.

The base TheSingularityWorkshop.FSM_Rest package remains framework-agnostic:
it owns REST descriptors, request construction, and transport.

This package is the optional composition boundary:

FSM_REST -> FSM_REST.COS -> FSM_COS

RestMicroBundle carries a RestApiDescriptor into an FSM_COS composition as
an executable MicroBundle capability.

The direction is intentional: REST does not need to know that FSM_COS exists.
FSM_REST.COS is where that integration is requested.

---

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop" aria-label="The Singularity Workshop living avatar">
    <img src="https://raw.githubusercontent.com/TrentBest/WebPage/development/TheSingularityWorkshop/wwwroot/Images/TheSingularityWorkshopLivingAvatar.svg" alt="The Singularity Workshop living avatar — a digital vortex converging on its central eye" width="112" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>If a picture is worth a thousand words, what is a living picture worth?</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://www.patreon.com/TheSingularityWorkshop">Support the Workshop</a>
</p>
