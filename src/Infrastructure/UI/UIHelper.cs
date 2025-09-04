using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using V1_Trade.Infrastructure.Telemetry;

namespace V1_Trade.Infrastructure.UI
{
    public class UIThrottle
    {
        private readonly int _minInterval;
        private readonly Stopwatch _sw = Stopwatch.StartNew();

        public UIThrottle(int minIntervalMs = 16)
        {
            _minInterval = minIntervalMs;
        }

        public async Task InvokeAsync(Func<Task> action)
        {
            var elapsed = _sw.ElapsedMilliseconds;
            if (elapsed < _minInterval)
                await Task.Delay(_minInterval - (int)elapsed).ConfigureAwait(false);

            var start = _sw.ElapsedMilliseconds;
            await action().ConfigureAwait(false);
            var duration = _sw.ElapsedMilliseconds - start;
            TelemetryClient.Instance.RecordUi(duration);
            _sw.Restart();
        }
    }

    public class GridBindingBuffer<T>
    {
        private List<T> _current = new List<T>();

        public (IEnumerable<T> added, IEnumerable<T> removed) Diff(IEnumerable<T> next)
        {
            var added = next.Except(_current).ToList();
            var removed = _current.Except(next).ToList();
            _current = next.ToList();
            return (added, removed);
        }
    }
}
