using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Forwarder;

namespace WebReactSpa.Internals
{
    internal class ProxyToDevServerMiddleware
    {
        private readonly IHttpForwarder yarpForwarder;
        private readonly RequestDelegate next;
        private readonly ILogger<ProxyToDevServerMiddleware> logger;
        private readonly HttpMessageInvoker httpClient;

        public ProxyToDevServerMiddleware(IHttpForwarder yarpForwarder, RequestDelegate next, ILogger<ProxyToDevServerMiddleware> logger)
        {
            
            this.yarpForwarder = yarpForwarder ?? throw new ArgumentNullException(nameof(yarpForwarder));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));


            this.httpClient = new HttpMessageInvoker(new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false,
                UseProxy = false
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                // This request will be handled by someone else already, skip proxying to the dev Next.js server...
                // Example scenario where we encounter this: a controller in the ASP .NET Core ap already claimed this route -- it should take precedence
                await this.next(context);
                return;
            }

#if NET6_0_OR_GREATER
            // This can be removed once we upgrade to YARP 2.0 (issue: https://github.com/microsoft/reverse-proxy/issues/1375)
            // See: https://github.com/microsoft/reverse-proxy/issues/1375#issuecomment-1366099983
            if (context.Request.Method == HttpMethods.Connect && context.Request.Protocol != HttpProtocol.Http11)
            {
                var resetFeature = context.Features.Get<IHttpResetFeature>();
                if (resetFeature != null)
                {
                    // See: https://www.rfc-editor.org/rfc/rfc7540#section-7
                    const int HTTP_1_1_REQUIRED = 0xd;
                    resetFeature.Reset(HTTP_1_1_REQUIRED);
                    return;
                }
            }
#endif

            var error = await this.yarpForwarder.SendAsync(context, "http://localhost:3000/", this.httpClient, new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromMinutes(5) });
            switch (error)
            {
                case ForwarderError.None:
                case ForwarderError.RequestCanceled:
                case ForwarderError.RequestBodyCanceled:
                case ForwarderError.ResponseBodyCanceled:
                case ForwarderError.UpgradeRequestCanceled:
                case ForwarderError.UpgradeResponseCanceled:
                    // Success, or deliberate client cancellation -- in any case, not our fault, so move on...
                    break;
                default:
                    // An actual error...
                    {
                        string message =
                            $"[NextjsStaticHosting.AspNetCore] Unable to reach Next.js dev server. Please ensure it is running at http://localhost:3000/.{Environment.NewLine}" +
                            $"If you are running in production and did not intend to proxy to a Next.js dev server, please ensure ProxyToDevServer settings is false.{Environment.NewLine}" +
                            $"YARP error: {error}";
                        this.logger.LogError(message);
                        if (!context.Response.HasStarted)
                        {
                            context.Response.ContentType = "text/plain";
                            await context.Response.WriteAsync(message);
                        }
                    }
                    break;
            }
        }
    }
}
