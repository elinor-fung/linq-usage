using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections;

namespace LinqUsage
{
    // https://github.com/dotnet/runtime/issues/76205#issuecomment-1258591573
    // https://github.com/dotnet/performance/tree/main/src/benchmarks/micro/libraries/System.Linq/Perf.Enumerable.cs
    internal class OrderBy
    {
        public static IEnumerable<T> WhereOrderBy_Linq<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> orderSelector)
            => collection.Where(predicate).OrderBy(orderSelector);

        public static IEnumerable<T> WhereOrderBy_NoLinqList<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> orderSelector)
        {
            var list = new List<T>();
            foreach (var i in collection)
            {
                if (predicate(i))
                    list.Add(i);
            }

            list.Sort((left, right) => Comparer<U>.Default.Compare(orderSelector(left), orderSelector(right)));
            return list;
        }

        public static T WhereOrderByFirst_Linq<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> orderSelector)
            => collection.Where(predicate).OrderBy(orderSelector).First();

        public static T WhereOrderByFirst_NoLinq<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> orderSelector)
        {
            T res;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out res);
            bool found = false;
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                if (!found)
                {
                    res = i;
                    found = true;
                }
                else if (Comparer<U>.Default.Compare(orderSelector(res), orderSelector(i)) > 0)
                {
                    res = i;
                }
            }

            if (found)
                return res;

            throw new InvalidOperationException();
        }

        public static T OrderByFirst_Linq<T, U>(IEnumerable<T> collection, Func<T, U> orderSelector)
            => collection.OrderBy(orderSelector).First();

        public static T OrderByFirst_NoLinq<T, U>(IEnumerable<T> collection, Func<T, U> orderSelector)
        {
            T res = default;
            bool found = false;
            foreach (var i in collection)
            {
                if (!found)
                {
                    res = i;
                    found = true;
                }
                else if (Comparer<U>.Default.Compare(orderSelector(res), orderSelector(i)) > 0)
                {
                    res = i;
                }
            }

            if (found)
                return res;

            throw new InvalidOperationException();
        }

        public static int OrderByFirst_PlusOne_NoLinq(IEnumerable<int> collection)
        {
            int res = default;
            bool found = false;
            foreach (var i in collection)
            {
                if (!found)
                {
                    res = i;
                    found = true;
                }
                else if (LinqTestData.Predicates.PlusOne(res) > LinqTestData.Predicates.PlusOne(i))
                {
                    res = i;
                }
            }

            if (found)
                return res;

            throw new InvalidOperationException();
        }

        public static IEnumerable<T> OrderBy_Linq<T, U>(IEnumerable<T> collection, Func<T, U> orderSelector)
            => collection.OrderBy(orderSelector);

        public static List<T> OrderBy_NoLinqList<T, U>(IEnumerable<T> collection, Func<T, U> orderSelector)
        {
            var list = new List<T>(collection);
            list.Sort((x, y) => Comparer<U>.Default.Compare(orderSelector(x), orderSelector(y)));
            return list;
        }

        public static T[] OrderBy_NoLinqArrayInputOutput<T, U>(T[] collection, Func<T, U> orderSelector)
        {
            var copy = new T[collection.Length];
            Array.Copy(collection, copy, collection.Length);
            Array.Sort(copy, (x, y) => Comparer<U>.Default.Compare(orderSelector(x), orderSelector(y)));
            return collection;
        }

        public static IEnumerable<T> Order_Linq<T>(IEnumerable<T> collection)
            => collection.Order();

        public static List<T> Order_NoLinqList<T>(IEnumerable<T> collection)
        {
            var list = new List<T>(collection);
            list.Sort();
            return list;
        }

        public static T[] Order_NoLinqArrayInputOutput<T>(T[] collection)
        {
            var copy = new T[collection.Length];
            Array.Copy(collection, copy, collection.Length);
            Array.Sort(copy);
            return collection;
        }

        public static T OrderFirst_Linq<T>(IEnumerable<T> collection)
            => collection.Order().First();

        public static T OrderFirst_NoLinq<T>(IEnumerable<T> collection)
        {
            T res = default;
            bool found = false;
            foreach (var i in collection)
            {
                if (!found)
                {
                    res = i;
                    found = true;
                }
                else if (Comparer<T>.Default.Compare(res, i) > 0)
                {
                    res = i;
                }
            }

            if (found)
                return res;

            throw new InvalidOperationException();
        }
    }
}
