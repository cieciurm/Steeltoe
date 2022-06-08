// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Common.Expression.Internal.Spring.SPR10210.Comp;
using Steeltoe.Common.Expression.Internal.Spring.SPR10210.Infra;

namespace Steeltoe.Common.Expression.Internal.Spring.SPR10210;

public abstract class A : B<IC>
{
    public void BridgeMethod()
    {
    }
}
