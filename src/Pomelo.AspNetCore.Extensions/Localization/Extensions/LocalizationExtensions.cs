﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.PlatformAbstractions;
using Pomelo.AspNetCore.Extensions.Localization;
using Pomelo.AspNetCore.Extensions.Localization.EntityFramework;
using Pomelo.AspNetCore.Extensions.Localization.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection self, string resourcePath = "/Localization")
        {
            return self
                .AddHttpContextAccessor()
                .AddSingleton<ILocalizationStringCollection>(x => new JsonCollection(resourcePath, x.GetRequiredService<IRequestCultureProvider>(), x.GetRequiredService<IHostingEnvironment>()));
        }

        public static IServiceCollection AddEFLocalization<TContext, TKey>(this IServiceCollection self)
            where TKey:IEquatable<TKey>
            where TContext : class, ILocalizationDbContext<TKey>
        {
            return self
                .AddHttpContextAccessor()
                .AddScoped<ILocalizationDbContext<TKey>, TContext>()
                .AddScoped<EFLocalizationManager<TKey>>()
                .AddSingleton<ILocalizationStringCollection, EFCollection<TKey>>();
        }

        public static IServiceCollection AddCookieCulture(this IServiceCollection self, string cookieField = "ASPNET_LANG")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new CookieRequestCultureProvider(x, cookieField));
        }

        public static IServiceCollection AddQueryCulture(this IServiceCollection self, string queryField = "lang")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new QueryStringRequestCultureProvider(x, queryField));
        }

        public static IServiceCollection AddRouteCulture(this IServiceCollection self, string routeField = "lang")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new RouteRequestCultureProvider(x, routeField));
        }
    }
}
