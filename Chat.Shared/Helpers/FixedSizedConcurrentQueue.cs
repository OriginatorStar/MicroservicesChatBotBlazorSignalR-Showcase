using System.Collections.Concurrent;

namespace Chat.Shared.Helpers
{
    public class FixedSizedConcurrentQueue<T> : ConcurrentQueue<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedConcurrentQueue(List<T> listToAdd, int size) : base(listToAdd)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
    }
}
