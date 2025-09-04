using System.Threading;

namespace V1_Trade.Infrastructure.Sequencing
{
    public class SequenceGate<T>
    {
        private long _updateSeq;
        private long _lastAppliedSeq;
        private T _latest = default!;

        public long UpdateSeq => Interlocked.Read(ref _updateSeq);
        public long LastAppliedSeq => Interlocked.Read(ref _lastAppliedSeq);

        public void Enqueue(T value)
        {
            _latest = value;
            Interlocked.Increment(ref _updateSeq);
        }

        public bool TryDequeue(out T value)
        {
            if (LastAppliedSeq < UpdateSeq)
            {
                value = _latest;
                Interlocked.Exchange(ref _lastAppliedSeq, UpdateSeq);
                return true;
            }

            value = default!;
            return false;
        }
    }
}
