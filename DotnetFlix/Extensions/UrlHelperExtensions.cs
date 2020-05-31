using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetFlix.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink<T>(this IUrlHelper urlHelper, T userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink<T>(this IUrlHelper urlHelper, T userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
