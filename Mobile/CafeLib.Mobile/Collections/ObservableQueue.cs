using System.Collections.Generic;
using System.Collections.ObjectModel;
// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Collections
{
    public class ObservableQueue<T> : ObservableCollection<T>
    {
        public ObservableQueue(int limit = -1)
        {
            Limit = limit > 0 ? limit : int.MaxValue;
        }

        public int Limit { get; set; }

        public ObservableQueue(IEnumerable<T> collection, int limit = -1)
            : this(limit)
        {
            foreach (var item in collection)
                Add(item);
        }

        public ObservableQueue(List<T> list, int limit = -1)
            : this(limit)
        {
            foreach (var item in list)
                Add(item);
        }

        public ObservableQueue(ObservableCollection<T> list, int limit = -1)
            : this(limit)
        {
            foreach (var item in list)
                if (Count < limit)
                {
                    Add(item);
                }
        }

        public virtual T Dequeue()
        {
            CheckReentrancy();
            var item = base[0];
            RemoveItem(0);
            return item;
        }

        public virtual void Enqueue(T item)
        {
            CheckReentrancy();
            lock (this)
            {
                while (Count >= Limit && TryDequeue(out _))
                {
                    // continue.
                }
            }

            Add(item);
        }

        protected bool TryDequeue(out T item)
        {
            CheckReentrancy();
            item = default;
            if (Count == 0) return false;
            item = Dequeue();
            return true;
        }
    }
}