using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace BotServer.SignalRServer
{
    public class LoggingPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            Debug.WriteLine($"=> Invoking {context.MethodDescriptor.Name} on hub {context.MethodDescriptor.Hub.Name}");
            return base.OnBeforeIncoming(context);
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            Debug.WriteLine($"<= Invoking {context.Invocation.Method} on client hub {context.Invocation.Hub}");
            return base.OnBeforeOutgoing(context);
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            Debug.WriteLine($"=> Exception {exceptionContext.Error.Message}");
            if (exceptionContext.Error.InnerException != null)
            {
                Debug.WriteLine($"=> Inner Exception {exceptionContext.Error.InnerException.Message}");
            }
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}
