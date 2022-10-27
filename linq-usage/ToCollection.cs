// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace LinqUsage
{
    // https://github.com/dotnet/performance/tree/main/src/benchmarks/micro/libraries/System.Linq/Perf.Enumerable.cs
    public class ToCollection
    {
        private readonly Consumer _consumer = new Consumer();

        // used by benchmarks that have no special handling per collection type
        public IEnumerable<object> IEnumerableArgument()
        {
            yield return LinqTestData.IEnumerable;
        }

        public IEnumerable<object> ToArrayArguments()
        {
            // ToArray() has two code paths: ICollection and IEnumerable
            // https://github.com/dotnet/corefx/blob/a10890f4ffe0fadf090c922578ba0e606ebdd16c/src/Common/src/System/Collections/Generic/EnumerableHelpers.Linq.cs#L93

            yield return LinqTestData.ICollection;
            yield return LinqTestData.IEnumerable;
        }

        // [Benchmark]
        [ArgumentsSource(nameof(ToArrayArguments))]
        public int[] ToArray(LinqTestData input) => input.Collection.ToArray();

        public IEnumerable<object> SelectToArrayArguments()
        {
            // Select().ToArray() has 5 code paths: SelectEnumerableIterator.ToArray, SelectArrayIterator.ToArray, SelectRangeIterator.ToArray, SelectListIterator.ToArray, SelectIListIterator.ToArray
            // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/Select.SpeedOpt.cs

            yield return LinqTestData.IEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.Range;
            yield return LinqTestData.List;
            yield return LinqTestData.IList;
        }

        // [Benchmark]
        [ArgumentsSource(nameof(SelectToArrayArguments))]
        public int[] SelectToArray(LinqTestData input) => input.Collection.Select(i => i + 1).ToArray();

        // ToList() has same 2 code paths as ToArray
        // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/ToCollection.cs#L30
        // https://github.com/dotnet/coreclr/blob/d61a380bbfde580986f416d8bf3e687104cd5701/src/System.Private.CoreLib/shared/System/Collections/Generic/List.cs#L61
        // [Benchmark]
        [ArgumentsSource(nameof(ToArrayArguments))]
        public List<int> ToList(LinqTestData input) => input.Collection.ToList();

        // Select().ToList() has same 5 code paths as Select.ToArray
        // [Benchmark]
        [ArgumentsSource(nameof(SelectToArrayArguments))]
        public List<int> SelectToList(LinqTestData input) => input.Collection.Select(i => i + 1).ToList();

        public IEnumerable<object> ToDictionaryArguments()
        {
            // ToDictionary() has 3 code paths: Array, List and IEnumerable
            // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/ToCollection.cs#L36

            yield return LinqTestData.IEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.List;
        }

        // [Benchmark]
        [ArgumentsSource(nameof(ToDictionaryArguments))]
        public Dictionary<int, int> ToDictionary(LinqTestData input) => input.Collection.ToDictionary(key => key);
    }
}