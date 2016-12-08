using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace BotServer.SignalRServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Catch unhandled exceptions
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            // Load accounts on the server
            BotServerHub.Server.Load();

            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                while (true)
                {
                    string command = Console.ReadLine();
                    if (command == "exit")
                        break;
                }
            }
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            // Allow CORS
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
