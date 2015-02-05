using System;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

namespace UserService
{
    public class SampleBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            context.IncomingLogicalMessage.Headers["NewHeader"] = Guid.NewGuid().ToString();
            next();
        }
    }
    class NewStepInPipeline : RegisterStep
    {
        public NewStepInPipeline()
            : base("NewStepInPipeline", typeof(SampleBehavior), "Logs a warning when processing takes too long")
        {

            // Optional: Specify where it needs to be invoked in the pipeline, for example InsertBefore or InsertAfter
            InsertBefore(WellKnownStep.InvokeHandlers);
        }
    }

    class NewStepInPipelineRegistration : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            // Register the new step in the pipeline
            configuration.Pipeline.Register<NewStepInPipeline>();
        }
    }
}
