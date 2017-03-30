﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> TopCore.Framework.DependencyInjection </Project>
//     <File>
//         <Name> Helper </Name>
//         <Created> 30 Mar 17 8:18:26 PM </Created>
//         <Key> 92d2515d-04f0-4d5f-a109-e7ba1655bb42 </Key>
//     </File>
//     <Summary>
//         Helper
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace TopCore.Framework.DependencyInjection
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyInjectionScanner(this IServiceCollection services)
        {
            services.AddSingleton<Scanner>();
            return services;
        }

        public static IServiceCollection ScanFromSelf(this IServiceCollection services)
        {
            var env = services.BuildServiceProvider().GetService<IHostingEnvironment>();
            services.ScanFromAssembly(new AssemblyName(env.ApplicationName));
            return services;
        }

        public static IServiceCollection ScanFromAssembly(this IServiceCollection services, AssemblyName assemblyName)
        {
            var scanner = services.GetScanner();
            scanner.RegisterAssembly(services, assemblyName);
            return services;
        }

        /// <summary>
        /// Scan and Register from all assemblies same location with Scanner
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ScanFromAllAssemblies(this IServiceCollection services)
        {
            var scanner = services.GetScanner();
            scanner.RegisterAllAssemblies(services);
            return services;
        }

        private static Scanner GetScanner(this IServiceCollection services)
        {
            var scanner = services.BuildServiceProvider().GetService<Scanner>();
            if (null == scanner)
            {
                throw new InvalidOperationException(
                    $"Unable to resolve {nameof(Scanner)}. Did you forget to call {nameof(services)}.{nameof(AddDependencyInjectionScanner)}?");
            }
            return scanner;
        }
    }
}