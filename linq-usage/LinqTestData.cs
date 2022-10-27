// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Collections = System.Collections;

namespace LinqUsage
{
    public class ItemCollection
    {
        public string Name { get; init; }
        public int Number { get; init; }
        public int[] Items { get; init; } = LinqTestData.ArrayOfInts;
        public ItemCollection(int n)
        {
            Name = $"Name#{n}";
            Number = n;
        }
    }

    public class LinqTestData
    {
        internal const int Size = 1000;

        internal static readonly int[] ArrayOfInts = CreateIntArray(Size);
        internal static readonly ItemCollection[] ArrayOfItemCollection = CreateItemCollectionArray(Size);
        internal static int LastElement = LinqTestData.ArrayOfInts[LinqTestData.Size - 1];

        internal static readonly LinqTestData Array = new LinqTestData(ArrayOfInts, ArrayOfItemCollection);
        internal static readonly LinqTestData List = new LinqTestData(new List<int>(ArrayOfInts), new List<ItemCollection>(ArrayOfItemCollection));
        internal static readonly LinqTestData Range = new LinqTestData(Enumerable.Range(0, Size));
        internal static readonly LinqTestData IEnumerable = new LinqTestData(new IEnumerableWrapper<int>(ArrayOfInts), new IEnumerableWrapper<ItemCollection>(ArrayOfItemCollection));
        internal static readonly LinqTestData IList = new LinqTestData(new IListWrapper<int>(ArrayOfInts), new IListWrapper<ItemCollection>(ArrayOfItemCollection));
        internal static readonly LinqTestData ICollection = new LinqTestData(new ICollectionWrapper<int>(ArrayOfInts), new ICollectionWrapper<ItemCollection>(ArrayOfItemCollection));
        internal static readonly LinqTestData IOrderedEnumerable = new LinqTestData(ArrayOfInts.OrderBy(i => i), ArrayOfItemCollection.OrderBy(c => c)); // OrderBy() returns IOrderedEnumerable (OrderedEnumerable is internal)

        public static class Predicates
        {
            public static int ReturnElement(int i) => i;

            public static int PlusOne(int i) => i + 1;
            public static int PlusOne(ItemCollection n) => n.Number + 1;

            public static bool MatchAll<T>(T i) => true;
            public static bool MatchNone<T>(T i) => false;

            public static bool MatchHalf(int i) => i % 2 == 0;
            public static bool MatchHalf(ItemCollection n) => n.Number % 2 == 0;

            public static bool FirstElementMatches(int i) => i == ArrayOfInts[0];
            public static bool FirstElementMatches(ItemCollection n) => n.Number == ArrayOfItemCollection[0].Number;

            public static bool LastElementMatches(int i) => i == LastElement;
            public static bool LastElementMatches(ItemCollection n) => n.Number == ArrayOfItemCollection[LinqTestData.Size - 1].Number;
        }

        private LinqTestData(IEnumerable<int> collection, IEnumerable<ItemCollection> itemCollection)
        {
            Collection = collection;
            ItemCollection = itemCollection;
        }

        private LinqTestData(IEnumerable<int> collection) : this(collection, Enumerable.Empty<ItemCollection>()) { }

        internal IEnumerable<int> Collection { get; }

        internal IEnumerable<ItemCollection> ItemCollection { get; }

        internal static int[] CreateIntArray(int size = Size)
        {
            var r = new Random(42);
            int[] arr = new int[size];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = r.Next();

            return arr;
        }

        internal static ItemCollection[] CreateItemCollectionArray(int size = Size)
        {
            var r = new Random(42);
            ItemCollection[] arr = new ItemCollection[size];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new ItemCollection(r.Next());

            return arr;
        }

        public override string ToString()
        {
            if (object.ReferenceEquals(this, Range)) // RangeIterator is a private type
                return "Range";

            switch (Collection)
            {
                case int[] _:
                    return "Array";
                case List<int> _:
                    return "List";
                case IList<int> _:
                    return "IList";
                case ICollection<int> _:
                    return "ICollection";
                case IOrderedEnumerable<int> _:
                    return "IOrderedEnumerable";
                default:
                    return "IEnumerable";
            }
        }

        private class IEnumerableWrapper<T> : IEnumerable<T>
        {
            private readonly T[] _array;
            public IEnumerableWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
        }

        private class ICollectionWrapper<T> : ICollection<T>
        {
            private readonly T[] _array;
            public ICollectionWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

            public int Count => _array.Length;
            public bool IsReadOnly => true;
            public bool Contains(T item) => System.Array.IndexOf(_array, item) >= 0;
            public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
        }

        private class IListWrapper<T> : IList<T>
        {
            private readonly T[] _array;
            public IListWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

            public int Count => _array.Length;
            public bool IsReadOnly => true;
            public T this[int index]
            {
                get { return _array[index]; }
                set { throw new NotImplementedException(); }
            }
            public bool Contains(T item) => System.Array.IndexOf(_array, item) >= 0;
            public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
            public int IndexOf(T item) => System.Array.IndexOf(_array, item);

            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
            public void Insert(int index, T item) => throw new NotImplementedException();
            public void RemoveAt(int index) => throw new NotImplementedException();
        }
    }
}
