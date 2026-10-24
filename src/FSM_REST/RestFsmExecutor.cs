using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Executes one REST request while recording its lifecycle in an FSM_API state machine.
/// </summary>
/// <remarks>
/// The FSM represents transport execution lifecycle, not HTTP business meaning: a received
/// 404 is a completed transport operation whose response reports a non-success status.
/// Each invocation owns its context and handle; the registered definition is shared.
/// </remarks>
public static class RestFsmExecutor
{
    private const string DefinitionName = "TheSingularityWorkshop.FSM_REST.Execution";
    private const string ProcessingGroup = "FSM_REST";

    static RestFsmExecutor()
    {
        TheSingularityWorkshop.FSM_API.FSM_API.Create.CreateProcessingGroup(ProcessingGroup);
        TheSingularityWorkshop.FSM_API.FSM_API.Create.CreateFiniteStateMachine(DefinitionName, 0, ProcessingGroup)
            .State("Ready", null, null, null)
            .State("Sending", null, null, null)
            .State("Completed", null, null, null)
            .State("Failed", null, null, null)
            .State("Cancelled", null, null, null)
            .Transition("Ready", "Sending", context => ((ExecutionContext)context).TargetState == "Sending")
            .Transition("Sending", "Completed", context => ((ExecutionContext)context).TargetState == "Completed")
            .Transition("Sending", "Failed", context => ((ExecutionContext)context).TargetState == "Failed")
            .Transition("Sending", "Cancelled", context => ((ExecutionContext)context).TargetState == "Cancelled")
            .BuildDefinition();
    }

    /// <summary>Sends a request and records Ready → Sending → Completed/Failed/Cancelled using FSM_API.</summary>
    public static async Task<RestResponse> SendAsync(
        RestRequest request,
        IRestTransport transport,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(transport);

        var context = new ExecutionContext();
        var handle = TheSingularityWorkshop.FSM_API.FSM_API.Create.CreateInstance(DefinitionName, context, ProcessingGroup);
        try
        {
            context.TargetState = "Sending";
            handle.Update(ProcessingGroup);
            if (handle.CurrentState != "Sending")
                throw new InvalidOperationException($"REST execution FSM did not enter Sending; state is '{handle.CurrentState}'.");

            try
            {
                var response = await transport.SendAsync(request, cancellationToken).ConfigureAwait(false);
                context.TargetState = "Completed";
                handle.Update(ProcessingGroup);
                if (handle.CurrentState != "Completed")
                    throw new InvalidOperationException($"REST execution FSM did not complete; state is '{handle.CurrentState}'.");
                return response;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                context.TargetState = "Cancelled";
                handle.Update(ProcessingGroup);
                throw;
            }
            catch
            {
                context.TargetState = "Failed";
                handle.Update(ProcessingGroup);
                throw;
            }
        }
        finally
        {
            TheSingularityWorkshop.FSM_API.FSM_API.Interaction.DestroyInstance(handle);
        }
    }

    private sealed class ExecutionContext : IStateContext
    {
        public string Name { get; set; } = "FSM_REST request execution";
        public bool IsValid { get; set; } = true;
        public string TargetState { get; set; } = "Ready";
    }
}
