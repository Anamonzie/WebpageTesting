using Serilog.Core;
using Serilog.Events;
using System.Collections.Concurrent;

namespace EpamWeb.Loggers
{
    public class InMemorySink : ILogEventSink
    {
        private readonly BlockingCollection<LogEvent> logEvents;

        public InMemorySink(BlockingCollection<LogEvent> logEvents)
        {
            this.logEvents = logEvents;
        }

        public void Emit(LogEvent logEvent)
        {
            logEvents.Add(logEvent);
        }
    }
}
