// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Configuration;

namespace Steeltoe.Common.Certificates;

public static class CertificateServiceCollectionExtensions
{
    /// <summary>
    /// Binds certificate paths in configuration to <see cref="CertificateOptions" /> for use with client certificates.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection" /> to add services to.
    /// </param>
    /// <param name="certificateName">
    /// Name of the certificate used in configuration and IOptions, or <see cref="string.Empty" /> for an unnamed certificate.
    /// </param>
    public static IServiceCollection ConfigureCertificateOptions(this IServiceCollection services, string? certificateName)
    {
        ArgumentGuard.NotNull(services);

        string configurationKey = string.IsNullOrEmpty(certificateName)
            ? CertificateOptions.ConfigurationKeyPrefix
            : ConfigurationPath.Combine(CertificateOptions.ConfigurationKeyPrefix, certificateName);

        services.AddOptions<CertificateOptions>().BindConfiguration(configurationKey);
        services.WatchFilePathInOptions<CertificateOptions>(configurationKey, certificateName, "CertificateFileName");
        services.WatchFilePathInOptions<CertificateOptions>(configurationKey, certificateName, "PrivateKeyFileName");

        services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<CertificateOptions>, ConfigureCertificateOptions>());
        return services;
    }
}
