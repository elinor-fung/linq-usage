using BenchmarkDotNet.Engines;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LinqUsage
{
    // Survey:
    // https://github.com/dotnet/runtime/issues/76205#issuecomment-1258591573
    // https://github.com/dotnet/runtime/issues/76205#issuecomment-1259029097
    // https://github.com/dotnet/runtime/issues/76205#issuecomment-1258600423
    //
    // Switched away:
    // https://github.com/space-wizards/space-station-14/pull/2011
    public class WhereSelect
    {
        public static IEnumerable<U> Select_Linq<T, U>(IEnumerable<T> collection, Func<T, U> selector) => collection.Select(selector);

        public static IEnumerable<U> Select_NoLinq<T, U>(IEnumerable<T> collection, Func<T, U> selector)
        {
            foreach (var i in collection)
            {
                yield return selector(i);
            }
        }

        public static List<U> Select_NoLinqList<T, U>(IEnumerable<T> collection, Func<T, U> selector)
        {
            var list = new List<U>();
            foreach (var i in collection)
            {
                list.Add(selector(i));
            }

            return list;
        }

        public static IEnumerable<U> Select_NoLinqListInput<T, U>(List<T> collection, Func<T, U> selector)
        {
            foreach (var i in collection)
                yield return selector(i);
        }

        public static IEnumerable<U> SelectWhere_Linq<T, U>(IEnumerable<T> collection, Func<U, bool> predicate, Func<T, U> selector)
            => collection.Select(selector).Where(predicate);

        public static IEnumerable<U> SelectWhere_NoLinq<T, U>(IEnumerable<T> collection, Func<U, bool> predicate, Func<T, U> selector)
        {
            foreach (var i in collection)
            {
                var s = selector(i);
                if (predicate(s))
                    yield return s;
            }
        }

        public static List<U> SelectWhere_NoLinqList<T, U>(IEnumerable<T> collection, Func<U, bool> predicate, Func<T, U> selector)
        {
            var list = new List<U>();
            foreach (var i in collection)
            {
                var s = selector(i);
                if (predicate(s))
                    list.Add(s);
            }

            return list;
        }

        public static IEnumerable<T> Where_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.Where(predicate);

        public static IEnumerable<T> Where_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    yield return i;
            }
        }

        public static List<T> Where_NoLinqList<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var i in collection)
            {
                if (predicate(i))
                    list.Add(i);
            }

            return list;
        }

        public static IEnumerable<T> WhereSelect_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
            => collection.Where(predicate).Select(selector);

        public static IEnumerable<T> WhereSelect_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    yield return selector(i);
            }
        }

        public static IEnumerable<T> WhereSelectChain2_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
            => collection
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector);

        public static IEnumerable<T> WhereSelectChain3_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
            => collection
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector);

        public static IEnumerable<T> WhereSelectChain4_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
            => collection
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector);

        public static IEnumerable<T> WhereSelectChain5_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
            => collection
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector)
                .Where(predicate).Select(selector);

        public static IEnumerable<T> WhereSelectChain2_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                var s = selector(i);
                if (predicate(s))
                    yield return selector(s);
            }
        }

        public static IEnumerable<T> WhereSelectChain3_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                var s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(s);
                if (predicate(s))
                    yield return selector(s);
            }
        }

        public static IEnumerable<T> WhereSelectChain4_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                var s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(s);
                if (predicate(s))
                    yield return selector(s);
            }
        }

        public static IEnumerable<T> WhereSelectChain5_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                var s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(i);
                if (!predicate(s))
                    continue;

                s = selector(s);
                if (predicate(s))
                    yield return selector(s);
            }
        }

        public static T[] WhereSelect_NoLinqArray<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            var result = new T[collection.Count()];
            int count = 0;
            foreach (var i in collection)
            {
                if (predicate(i))
                {
                    result[count] = selector(i);
                    count++;
                }
            }

            Array.Resize(ref result, count);
            return result;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<T> WhereSelect_NoLinqArrayInput<T>(T[] collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    yield return selector(i);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T[] WhereSelect_NoLinqArrayInputOutput<T>(T[] collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            var result = new T[collection.Count()];
            int count = 0;
            foreach (var i in collection)
            {
                if (predicate(i))
                {
                    result[count] = selector(i);
                    count++;
                }
            }

            Array.Resize(ref result, count);
            return result;
        }

        public static List<T> WhereSelect_NoLinqList<T>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            var list = new List<T>();
            foreach (var i in collection)
            {
                if (predicate(i))
                    list.Add(selector(i));
            }

            return list;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<T> WhereSelect_NoLinqListInput<T>(List<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    yield return selector(i);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static List<T> WhereSelect_NoLinqListInputOutput<T>(List<T> collection, Func<T, bool> predicate, Func<T, T> selector)
        {
            var list = new List<T>(collection.Count);
            foreach (var i in collection)
            {
                if (predicate(i))
                    list.Add(selector(i));
            }
            return list;
        }

        public static U WhereSelectFirst_Linq<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> selector)
            => collection.Where(predicate).Select(selector).First();

        public static U WhereSelectFirst_NoLinq<T, U>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> selector)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    return selector(i);
            }

            throw new InvalidOperationException();
        }

        public static U WhereSelectOrderByFirst_Linq<T, U, V>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> selector, Func<U, V> orderSelector)
            => collection.Where(predicate).Select(selector).OrderBy(orderSelector).First();

        public static U WhereSelectOrderByFirst_NoLinq<T, U, V>(IEnumerable<T> collection, Func<T, bool> predicate, Func<T, U> selector, Func<U, V> orderSelector)
        {
            U res;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out res);
            bool found = false;
            foreach (var i in collection)
            {
                if (!predicate(i))
                    continue;

                var s = selector(i);
                if (!found)
                {
                    res = s;
                    found = true;
                }
                else if (Comparer<V>.Default.Compare(orderSelector(res), orderSelector(s)) > 0)
                {
                    res = s;
                }
            }

            if (found)
                return res;

            throw new InvalidOperationException();
        }

        public static T WhereFirst_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.Where(predicate).First();

        public static T WhereFirst_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    return i;
            }

            throw new InvalidOperationException();
        }

        public static bool WhereAny_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.Where(predicate).Any();

        public static bool WhereAny_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    return true;
            }

            return false;
        }
    }
}