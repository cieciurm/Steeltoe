// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Steeltoe.Bootstrap.Autoconfig;

internal static class AssemblyExtensions
{
    internal static IEnumerable<string> ExcludedAssemblies { get; set; }

    private static readonly HashSet<string> MissingAssemblies = new ();

    internal static Assembly LoadAnyVersion(object sender, ResolveEventArgs args)
    {
        // Load whatever version available - strip out version and culture info
        static string GetSimpleName(string assemblyName) => new Regex(",.*").Replace(assemblyName, string.Empty);

        var name = GetSimpleName(args.Name);
        if (MissingAssemblies.Contains(name))
        {
            return null;
        }

        // AssemblyName.Equals() returns false when path and full name are identical, so the code below
        // avoids a crash caused by inserting duplicate keys in dictionary.
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .GroupBy(asm => asm.GetName().Name)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

        if (assemblies.TryGetValue(name, out var assembly))
        {
            return assembly;
        }

        if (args.Name.Contains(".resources"))
        {
            return args.RequestingAssembly;
        }

        MissingAssemblies.Add(name); // throw it in there to prevent recursive attempts to resolve
        assembly = Assembly.Load(name);
        MissingAssemblies.Remove(name);
        return assembly;
    }

    internal static bool IsAssemblyLoaded(string assemblyName)
    {
        if (ExcludedAssemblies.Contains(assemblyName))
        {
            return false;
        }

        return ReflectionHelpers.IsAssemblyLoaded(assemblyName);
    }

    internal static bool IsEitherAssemblyLoaded(string assemblyName1, string assemblyName2) =>
        IsAssemblyLoaded(assemblyName1) || IsAssemblyLoaded(assemblyName2);
}