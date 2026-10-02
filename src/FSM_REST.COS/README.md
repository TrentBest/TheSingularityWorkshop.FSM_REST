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
