using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using V1_Trade.Infrastructure.Telemetry;

namespace V1_Trade.Infrastructure.Eventing
{
    public interface IEventBus
    {
        ValueTask PublishAsync(object evt, CancellationToken ct = default);
        IAsyncEnumerable<object> EventsAsync(CancellationToken ct = default);
    }

    public class InProcEventBus : IEventBus
    {
        private readonly Channel<object> _channel;

        public InProcEventBus(int capacity = 1024)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<object>(options);
        }

        public async ValueTask PublishAsync(object evt, CancellationToken ct = default)
        {
            await _channel.Writer.WriteAsync(evt, ct).ConfigureAwait(false);
            TelemetryClient.Instance.IncrementEvents();
        }

        public IAsyncEnumerable<object> EventsAsync(CancellationToken ct = default)
        {
            return _channel.Reader.ReadAllAsync(ct);
        }
    }
}
