﻿using Ambacht.Common.Authentication;
using Ambacht.Common.Blazor.Services;
using Ambacht.Common.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Ambacht.Common.Blazor
{
    public static class DI
    {

        public static void AddAmbachtCommonBlazor(this IServiceCollection services, string baseAddress)
        {
            services.AddTransient<IDownloadGeneratedFileService, DownloadGeneratedFileService>();
            services.AddTransient<ILocalStorageService, LocalStorageService>();
            services.AddTransient<ISetWindowLocationService, SetWindowLocationService>();
            services.AddTransient<IJavascriptUtilService, JavascriptUtilService>();
            services.AddSingleton<IHeaderService, HeaderService>();
            services.AddSingleton<IToastService, ToastService>();
            services.AddTransient<IUserActionHandler, UserActionHandler>();
            services.AddTransient<IGetSizeService, JavascriptGetSizeService>();
            services.AddTransient<ITextMeasurementService, TextMeasurementService>();
            services.AddTransient<IPrincipalService, BlazorPrincipalService>();
        }

    }
}