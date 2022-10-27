using BenchmarkDotNet.Running;
using LinqUsage;

if (args.Length >= 1 && args[0] == "benchmarks")
{
    var summary = BenchmarkRunner.Run<Benchmarks>(args: args[1..]);
}
else
{
    int[] data = (int[])LinqTestData.Array.Collection;
    //List<int> data = (List<int>)LinqTestData.List.Collection;
    //IEnumerable<int> data = LinqTestData.IEnumerable.Collection;
    //List<ItemCollection> data = (List<ItemCollection>)LinqTestData.List.ItemCollection;
    //IEnumerable<ItemCollection> data = LinqTestData.IEnumerable.ItemCollection;

    Func<int, bool> predicate = LinqTestData.Predicates.MatchAll;
    Func<int, int> selector = LinqTestData.Predicates.PlusOne;
    Func<int, int> orderSelector = LinqTestData.Predicates.ReturnElement;

    Console.WriteLine("Press any key");
    Console.ReadLine();

    WithLinq(data);
    NoLinq(data);
}

static void WithLinq(IEnumerable<int> collection)
{
    int i = OrderBy.OrderByFirst_Linq(collection, LinqTestData.Predicates.ReturnElement);
    Test.count += i;

    // foreach (var ll in l) { Test.count += ll; }
}

static void NoLinq(IEnumerable<int> collection)
{
    int i = OrderBy.OrderByFirst_NoLinq(collection, LinqTestData.Predicates.ReturnElement);
    Test.count += i;

    // foreach (var ll in l) { Test.count += ll; }
}

static class Test
{
    internal static int count = 0;
}
