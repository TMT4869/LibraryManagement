using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace FA.JustBlog.API.Exceptions;

/// <summary>
///     The exception middleware extension class
/// </summary>
public static class ExceptionMiddlewareExtension
{
    /// <summary>
    ///     Configures the build in exception handle using the specified app
    /// </summary>
    /// <param name="app">The app</param>
    public static void ConfigureBuildInExceptionHandle(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var contextRequest = context.Features.Get<IHttpRequestFeature>();

                if (contextFeature != null)
                    await context.Response.WriteAsync(new ErrorVM
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Path = contextRequest.Path
                    }.ToString());
            });
        });
    }
}