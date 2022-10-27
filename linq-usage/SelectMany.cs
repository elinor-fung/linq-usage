namespace LinqUsage
{
    // https://github.com/dotnet/runtime/pull/44923
    internal class SelectMany
    {
        public static IEnumerable<int> SelectMany_Linq(IEnumerable<ItemCollection> collection)
            => collection.SelectMany(i => i.Items);

        public static IEnumerable<int> SelectMany_NoLinq(IEnumerable<ItemCollection> collection)
        {
            foreach (var c in collection)
            {
                foreach (var i in c.Items)
                    yield return i;
            }
        }

        public static List<int> SelectMany_NoLinqList(IEnumerable<ItemCollection> collection)
        {
            var list = new List<int>();
            foreach (var c in collection)
            {
                foreach (var i in c.Items)
                    list.Add(i);
            }

            return list;
        }

        public static IEnumerable<(int, int)> SelectMany_Collection_Linq(IEnumerable<ItemCollection> collection)
            => collection.SelectMany(i => i.Items, (i, c) => (i.Number, c));

        public static IEnumerable<(int, int)> SelectMany_Collection_NoLinq(IEnumerable<ItemCollection> collection)
        {
            foreach (var c in collection)
            {
                foreach (var i in c.Items)
                    yield return (c.Number, i);
            }
        }

        public static List<(int, int)> SelectMany_Collection_NoLinqList(IEnumerable<ItemCollection> collection)
        {
            var list = new List<(int, int)>();
            foreach(var c in collection)
            {
                foreach (var i in c.Items)
                    list.Add((c.Number, i));
            }

            return list;
        }
    }
}
