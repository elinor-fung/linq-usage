// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections;

using static LinqUsage.LinqTestData.Predicates;

namespace LinqUsage
{
    [MemoryDiagnoser(displayGenColumns: false)]
    [DisassemblyDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Method)]
    [HideColumns("Error", "StdDev", "Median", "RatioSD")]
    public class Benchmarks
    {
        private readonly Consumer _consumer = new Consumer();

        public static IEnumerable<object> SelectArguments()
        {
            // Select() has 4 code paths: SelectEnumerableIterator, SelectArrayIterator, SelectListIterator, SelectIListIterator
            // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/Select.cs

            yield return LinqTestData.IEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.List;
            yield return LinqTestData.IList;
        }

        [Benchmark]
        [ArgumentsSource(nameof(SelectArguments))]
        public void Select_Linq(LinqTestData input) => WhereSelect.Select_Linq(input.Collection, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(SelectArguments))]
        public void Select_NoLinq(LinqTestData input) => WhereSelect.Select_NoLinq(input.Collection, PlusOne).Consume(_consumer);

        [Benchmark]
        public void Select_NoLinqListInput() => WhereSelect.Select_NoLinqListInput((List<int>)LinqTestData.List.Collection, PlusOne).Consume(_consumer);

        public static IEnumerable<object> WhereArguments()
        {
            // Where() has 3 code paths: WhereEnumerableIterator, WhereArrayIterator, WhereListIterator
            // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/Where.cs

            yield return LinqTestData.IEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.List;
        }

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchAll_Linq(LinqTestData input) => WhereSelect.Where_Linq(input.Collection, MatchAll).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchAll_NoLinqList(LinqTestData input) => WhereSelect.Where_NoLinqList(input.Collection, MatchAll).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchAll_NoLinqYield(LinqTestData input) => WhereSelect.Where_NoLinq(input.Collection, MatchAll).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchNone_Linq(LinqTestData input) => WhereSelect.Where_Linq(input.Collection, MatchNone).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchNone_NoLinqList(LinqTestData input) => WhereSelect.Where_NoLinqList(input.Collection, MatchNone).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void Where_MatchNone_NoLinqYield(LinqTestData input) => WhereSelect.Where_NoLinq(input.Collection, MatchNone).Consume(_consumer);

        // Where().Select() has 3 code paths: WhereSelectEnumerableIterator, WhereSelectArrayIterator, WhereSelectListIterator, exactly as Where
        // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/Where.cs
        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelect_Linq(LinqTestData input) => WhereSelect.WhereSelect_Linq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelect_NoLinq(LinqTestData input) => WhereSelect.WhereSelect_NoLinq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain2_Linq(LinqTestData input) => WhereSelect.WhereSelectChain2_Linq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain2_NoLinq(LinqTestData input) => WhereSelect.WhereSelectChain2_NoLinq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain3_Linq(LinqTestData input) => WhereSelect.WhereSelectChain3_Linq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain3_NoLinq(LinqTestData input) => WhereSelect.WhereSelectChain3_NoLinq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain4_Linq(LinqTestData input) => WhereSelect.WhereSelectChain4_Linq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain4_NoLinq(LinqTestData input) => WhereSelect.WhereSelectChain4_NoLinq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain5_Linq(LinqTestData input) => WhereSelect.WhereSelectChain5_Linq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereSelectChain5_NoLinq(LinqTestData input) => WhereSelect.WhereSelectChain5_NoLinq(input.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        public void WhereSelect_NoLinqListInput() => WhereSelect.WhereSelect_NoLinqListInput((List<int>)LinqTestData.List.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        public void WhereSelect_NoLinqListInputOutput()
        {
            List<int> list = WhereSelect.WhereSelect_NoLinqListInputOutput((List<int>)LinqTestData.List.Collection, MatchAll, PlusOne);
            foreach (var item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void WhereSelect_NoLinqArrayInput() => WhereSelect.WhereSelect_NoLinqArrayInput((int[])LinqTestData.Array.Collection, MatchAll, PlusOne).Consume(_consumer);

        [Benchmark]
        public void WhereSelect_NoLinqArrayInputOutput()
        {
            int[] list = WhereSelect.WhereSelect_NoLinqArrayInputOutput((int[])LinqTestData.Array.Collection, MatchAll, PlusOne);
            foreach (var item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public List<int> WhereSelectToList_Linq(LinqTestData input) => WhereSelect.WhereSelect_Linq(input.Collection, MatchAll, PlusOne).ToList();

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public List<int> WhereSelectToList_NoLinq(LinqTestData input) => WhereSelect.WhereSelect_NoLinqList(input.Collection, MatchAll, PlusOne);

        [Benchmark]
        public void WhereSelectToList_NoLinqListInput() => WhereSelect.WhereSelect_NoLinqListInputOutput((List<int>)LinqTestData.List.Collection, MatchAll, PlusOne);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int[] WhereSelectToArray_Linq(LinqTestData input) => WhereSelect.WhereSelect_Linq(input.Collection, MatchAll, PlusOne).ToArray();

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int[] WhereSelectToArray_NoLinq(LinqTestData input) => WhereSelect.WhereSelect_NoLinqArray(input.Collection, MatchAll, PlusOne);

        [Benchmark]
        public void WhereSelectToArray_NoLinqArrayInput() => WhereSelect.WhereSelect_NoLinqArrayInputOutput((int[])LinqTestData.Array.Collection, MatchAll, PlusOne);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectFirst_LastElementMatches_Linq(LinqTestData input) => WhereSelect.WhereSelectFirst_Linq(input.Collection, LastElementMatches, PlusOne);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectFirst_FirstElementMatches_Linq(LinqTestData input) => WhereSelect.WhereSelectFirst_Linq(input.Collection, FirstElementMatches, PlusOne);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectFirst_LastElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereSelectFirst_NoLinq(input.Collection, LastElementMatches, PlusOne);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectFirst_FirstElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereSelectFirst_NoLinq(input.Collection, FirstElementMatches, PlusOne);

        // Where().First() has no special treatment, the code execution paths are based on WhereIterators
        // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/First.cs
        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereFirst_LastElementMatches_Linq(LinqTestData input) => WhereSelect.WhereFirst_Linq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereFirst_FirstElementMatches_Linq(LinqTestData input) => WhereSelect.WhereFirst_Linq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereFirst_LastElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereFirst_NoLinq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereFirst_FirstElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereFirst_NoLinq(input.Collection, FirstElementMatches);

        // Where().Any() has no special treatment, the code execution paths are based on WhereIterators
        // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/AnyAll.cs
        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public bool WhereAny_LastElementMatches_Linq(LinqTestData input) => WhereSelect.WhereAny_Linq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public bool WhereAny_FirstElementMatches_Linq(LinqTestData input) => WhereSelect.WhereAny_Linq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public bool WhereAny_LastElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereAny_NoLinq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public bool WhereAny_FirstElementMatches_NoLinq(LinqTestData input) => WhereSelect.WhereAny_NoLinq(input.Collection, FirstElementMatches);

        public static IEnumerable<object> FirstPredicateArguments()
        {
            // First(predicate) has 4 code paths: OrderedEnumerable, Array, List, and IEnumerable
            // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/First.cs

            yield return LinqTestData.IOrderedEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.List;
            yield return LinqTestData.IEnumerable;
        }

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public int First_LastElementMatches_Linq(LinqTestData input) => FirstSingleAny.First_Linq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public int First_LastElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.First_LastElementMatches_NoLinq(input.Collection);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public int First_LastElementMatches_NoLinqPredicate(LinqTestData input) => FirstSingleAny.First_NoLinq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public int First_FirstElementMatches_Linq(LinqTestData input) => FirstSingleAny.First_Linq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public int First_FirstElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.First_NoLinq(input.Collection, FirstElementMatches);

        // Any uses TryGetFirst internally.
        // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/First.cs
        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public bool Any_LastElementMatches_Linq(LinqTestData input) => FirstSingleAny.Any_Linq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public bool Any_LastElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.Any_NoLinq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public bool Any_FirstElementMatches_Linq(LinqTestData input) => FirstSingleAny.Any_Linq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(FirstPredicateArguments))]
        public bool Any_FirstElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.Any_NoLinq(input.Collection, FirstElementMatches);

        public static IEnumerable<object> SinglePredicateArguments()
        {
            // Single(predicate) has 3 code paths: Array, List, and IEnumerable
            // https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/First.cs

            yield return LinqTestData.Array;
            yield return LinqTestData.List;
            yield return LinqTestData.IEnumerable;
        }

        [Benchmark]
        [ArgumentsSource(nameof(SinglePredicateArguments))]
        public int Single_FirstElementMatches_Linq(LinqTestData input) => FirstSingleAny.Single_Linq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(SinglePredicateArguments))]
        public int Single_LastElementMatches_Linq(LinqTestData input) => FirstSingleAny.Single_Linq(input.Collection, LastElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(SinglePredicateArguments))]
        public int Single_FirstElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.Single_NoLinq(input.Collection, FirstElementMatches);

        [Benchmark]
        [ArgumentsSource(nameof(SinglePredicateArguments))]
        public int Single_LastElementMatches_NoLinq(LinqTestData input) => FirstSingleAny.Single_NoLinq(input.Collection, LastElementMatches);

        // OrderBy() has no special treatment and it has a single execution path
        // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/OrderBy.cs
        [Benchmark]
        public void OrderBy_Linq() => OrderBy.OrderBy_Linq(LinqTestData.IEnumerable.Collection, PlusOne).Consume(_consumer);

        [Benchmark]
        public void OrderBy_NoLinq()
        {
            List<int> list = OrderBy.OrderBy_NoLinqList(LinqTestData.IEnumerable.Collection, PlusOne);
            foreach (int item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void OrderBy_NoLinqList()
        {
            List<int> list = OrderBy.OrderBy_NoLinqList(LinqTestData.List.Collection, PlusOne);
            foreach (int item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void OrderBy_NoLinqArray()
        {
            int[] arr = OrderBy.OrderBy_NoLinqArrayInputOutput((int[])LinqTestData.Array.Collection, PlusOne);
            foreach (int item in arr)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void Order_Linq() => OrderBy.Order_Linq(LinqTestData.IEnumerable.Collection).Consume(_consumer);

        [Benchmark]
        public void Order_NoLinq()
        {
            List<int> list = OrderBy.Order_NoLinqList(LinqTestData.IEnumerable.Collection);
            foreach (int item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void Order_NoLinqList()
        {
            List<int> list = OrderBy.Order_NoLinqList(LinqTestData.List.Collection);
            foreach (int item in list)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public void Order_NoLinqArray()
        {
            int[] arr= OrderBy.Order_NoLinqArrayInputOutput((int[])LinqTestData.Array.Collection);
            foreach (int item in arr)
            {
                int value = item;
                _consumer.Consume(in value);
            }
        }

        [Benchmark]
        public List<int> OrderByToList_Linq() => OrderBy.OrderBy_Linq(LinqTestData.IEnumerable.Collection, ReturnElement).ToList();

        [Benchmark]
        public List<int> OrderByToList_NoLinq() => OrderBy.OrderBy_NoLinqList(LinqTestData.IEnumerable.Collection, ReturnElement);

        [Benchmark]
        public List<int> OrderByToList_LinqList() => OrderBy.OrderBy_Linq(LinqTestData.List.Collection, ReturnElement).ToList();

        [Benchmark]
        public List<int> OrderByToList_NoLinqList() => OrderBy.OrderBy_NoLinqList(LinqTestData.List.Collection, ReturnElement);

        [Benchmark]
        public int[] OrderByToArray_LinqArray() => OrderBy.OrderBy_Linq(LinqTestData.Array.Collection, ReturnElement).ToArray();

        [Benchmark]
        public int[] OrderByToArray_NoLinqArray() => OrderBy.OrderBy_NoLinqArrayInputOutput((int[])LinqTestData.Array.Collection, ReturnElement);

        [Benchmark]
        public int OrderByFirst_Linq() => OrderBy.OrderByFirst_Linq(LinqTestData.IEnumerable.Collection, PlusOne);

        [Benchmark]
        public int OrderByFirst_NoLinq() => OrderBy.OrderByFirst_NoLinq(LinqTestData.IEnumerable.Collection, PlusOne);
        
        [Benchmark]
        public int OrderByFirst_PlusOne_NoLinq() => OrderBy.OrderByFirst_PlusOne_NoLinq(LinqTestData.IEnumerable.Collection);

        [Benchmark]
        public int OrderFirst_Linq() => OrderBy.OrderFirst_Linq(LinqTestData.IEnumerable.Collection);

        [Benchmark]
        public int OrderFirst_NoLinq() => OrderBy.OrderFirst_NoLinq(LinqTestData.IEnumerable.Collection);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereOrderByToList_Linq(LinqTestData input) => OrderBy.WhereOrderBy_Linq(input.Collection, MatchAll, ReturnElement).ToList();

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public void WhereOrderByToList_NoLinq(LinqTestData input) => OrderBy.WhereOrderBy_NoLinqList(input.Collection, MatchAll, ReturnElement);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereOrderByFirst_Linq(LinqTestData input) => OrderBy.WhereOrderByFirst_Linq(input.Collection, MatchAll, ReturnElement);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereOrderByFirst_NoLinq(LinqTestData input) => OrderBy.WhereOrderByFirst_NoLinq(input.Collection, MatchAll, ReturnElement);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectOrderByFirst_Linq(LinqTestData input) => WhereSelect.WhereSelectOrderByFirst_Linq(input.Collection, MatchAll, PlusOne, ReturnElement);

        [Benchmark]
        [ArgumentsSource(nameof(WhereArguments))]
        public int WhereSelectOrderByFirst_NoLinq(LinqTestData input) => WhereSelect.WhereSelectOrderByFirst_NoLinq(input.Collection, MatchAll, PlusOne, ReturnElement);

        [Benchmark]
        public void SelectMany_Linq() => SelectMany.SelectMany_Linq(LinqTestData.IEnumerable.ItemCollection).Consume(_consumer);

        [Benchmark]
        public void SelectMany_NoLinq() => SelectMany.SelectMany_NoLinq(LinqTestData.IEnumerable.ItemCollection).Consume(_consumer);

        [Benchmark]
        public List<int> SelectManyToList_Linq() => SelectMany.SelectMany_Linq(LinqTestData.IEnumerable.ItemCollection).ToList();

        [Benchmark]
        public List<int> SelectManyToList_NoLinq() => SelectMany.SelectMany_NoLinqList(LinqTestData.IEnumerable.ItemCollection);

        [Benchmark]
        public void SelectMany_Collection_Linq() => SelectMany.SelectMany_Collection_Linq(LinqTestData.IEnumerable.ItemCollection).Consume(_consumer);

        [Benchmark]
        public void SelectMany_Collection_NoLinq() => SelectMany.SelectMany_Collection_NoLinq(LinqTestData.IEnumerable.ItemCollection).Consume(_consumer);

        [Benchmark]
        public List<(int, int)> SelectManyToList_Collection_Linq() => SelectMany.SelectMany_Collection_Linq(LinqTestData.IEnumerable.ItemCollection).ToList();

        [Benchmark]
        public List<(int, int)> SelectManyToList_Collection_NoLinq() => SelectMany.SelectMany_Collection_NoLinqList(LinqTestData.IEnumerable.ItemCollection);
    }
}