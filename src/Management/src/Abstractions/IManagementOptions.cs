﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Steeltoe.Management
{
    public interface IManagementOptions
    {
        bool? Enabled { get; }

        string Path { get; }

        List<IEndpointOptions> EndpointOptions { get; }

        public bool UseStatusCodeFromResponse { get; set; }
    }
}
