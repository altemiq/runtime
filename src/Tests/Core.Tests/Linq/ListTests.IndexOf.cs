// -----------------------------------------------------------------------
// <copyright file="ListTests.IndexOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ListTests
{
    [Theory]
    [MemberData(nameof(GetLists))]
    public void IndexOf(object first, object second)
    {
        TestListList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
        TestReadOnlyListReadOnlyList<int, int>(first, second, (f, s) => f.IndexOf(s), 1).Should().Be(1);
    }
}
