﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SystemCommonLibrary.Network;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class TokenAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTokenAuthentication(this IServiceCollection services,  
            Func<int, string, HttpClientType, bool> checkToken,
            Func<string, string, bool> checkPrvlg)
        {
            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Token";
                options.DefaultChallengeScheme = "Token";
            })
            .AddTokenAuthenticationHandler("Token", o => {
                o.CheckAuth = checkToken;
                o.CheckPrvlg = checkPrvlg;
            });
        }

        private static AuthenticationBuilder AddTokenAuthenticationHandler(this AuthenticationBuilder builder, string authenticationScheme, Action<TokenAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(authenticationScheme, configureOptions);
        }
    }
}
