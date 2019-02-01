using System.Collections.Generic;
using System.Collections.Specialized;

namespace SchoolsApp.Infrastructure
{
    public interface IObservableCollection<T> : IEnumerable<T>
    {
        int Count { get; }
        void Add(T item);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int index);
        int IndexOf(T item);
        void Insert(int index, T item);
        void Move(int oldIndex, int newIndex);
        bool Remove(T item);
        void RemoveAt(int index);
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
