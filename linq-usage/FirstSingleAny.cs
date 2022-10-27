// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace LinqUsage
{
    public class FirstSingleAny
    {
        public static T First_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.First(predicate);

        public static T First_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    return i;
            }

            throw new InvalidOperationException();
        }

        public static int First_LastElementMatches_NoLinq(IEnumerable<int> collection)
        {
            foreach (var i in collection)
            {
                if (LinqTestData.Predicates.LastElementMatches(i))
                    return i;
            }

            throw new InvalidOperationException();
        }

        public static bool Any_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.Any(predicate);

        public static bool Any_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            foreach (var i in collection)
            {
                if (predicate(i))
                    return true;
            }

            return false;
        }

        public static T Single_Linq<T>(IEnumerable<T> collection, Func<T, bool> predicate) => collection.Single(predicate);

        public static T Single_NoLinq<T>(IEnumerable<T> collection, Func<T, bool> predicate)
        {
            bool found = false;
            T res;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out res);
            foreach (var i in collection)
            {
                if (predicate(i))
                {
                    if (found)
                        throw new InvalidOperationException();

                    res = i;
                    found = true;
                }
            }

            if (!found)
                throw new InvalidOperationException();

            return res;
        }
    }
}