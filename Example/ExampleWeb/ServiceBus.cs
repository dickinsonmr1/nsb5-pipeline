using NServiceBus;
namespace ExampleWeb
{
    public static class ServiceBus
    {
        public static IBus Bus { get; private set; }
        private static readonly object padlock = new object();
        public static void Init()
        {
            if (Bus != null)
                return;
            lock (padlock)
            {
                if (Bus != null)
                    return;
                var cfg = new BusConfiguration();
                cfg.UseTransport<MsmqTransport>();
                cfg.UsePersistence<InMemoryPersistence>();
                cfg.EndpointName("ExampleWeb");
                cfg.PurgeOnStartup(true);
                cfg.EnableInstallers();
                cfg.Conventions()
                    .DefiningCommandsAs(
                        t =>
                            typeof (ICommand).IsAssignableFrom(t) ||
                            (t.Namespace != null && t.Namespace.EndsWith("Commands")) ||
                            (t.Assembly.FullName.Contains("Commands")));
                cfg.Conventions()
                    .DefiningEventsAs(
                        t =>
                            typeof(IEvent).IsAssignableFrom(t) ||
                            (t.Namespace != null && t.Namespace.EndsWith("Events")) ||
                            (t.Assembly.FullName.Contains("Events")));
                Bus = NServiceBus.Bus.Create(cfg).Start();
            }
        }
    }
}