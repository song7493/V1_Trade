using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace V1_Trade.Infrastructure.Telemetry
{
    public class TelemetryClient
    {
        public static TelemetryClient Instance { get; } = new TelemetryClient();

        private long _eventCount;
        private readonly List<long> _uiDurations = new List<long>();
        private readonly Stopwatch _sw = Stopwatch.StartNew();
        private readonly object _lock = new object();

        public void IncrementEvents()
        {
            Interlocked.Increment(ref _eventCount);
        }

        public void RecordUi(long durationMs)
        {
            lock (_lock)
            {
                _uiDurations.Add(durationMs);
                if (_uiDurations.Count > 1000)
                    _uiDurations.RemoveAt(0);
            }
        }

        public double EventsPerSecond
        {
            get
            {
                var seconds = _sw.Elapsed.TotalSeconds;
                if (seconds <= 0) return 0;
                return _eventCount / seconds;
            }
        }

        public double UiP95
        {
            get
            {
                lock (_lock)
                {
                    if (_uiDurations.Count == 0)
                        return 0;
                    var ordered = _uiDurations.OrderBy(x => x).ToArray();
                    var index = (int)Math.Ceiling(ordered.Length * 0.95) - 1;
                    return ordered[index];
                }
            }
        }
    }
}
