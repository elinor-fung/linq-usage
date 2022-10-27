// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections;

namespace LinqUsage
{
    // https://github.com/dotnet/performance/tree/main/src/benchmarks/micro/libraries/System.Linq/Perf.Enumerable.cs
    public class GroupBy
    {
        public static IEnumerable GroupBy_Linq(IEnumerable<int> collection) => collection.GroupBy(x => x % 10);
    }
}