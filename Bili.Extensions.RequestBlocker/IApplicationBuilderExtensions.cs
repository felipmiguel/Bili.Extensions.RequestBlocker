using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Bili.Extensions.RequestBlocker
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBlocker(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                IEnumerable<IRequestBlocker> blockers = context.RequestServices.GetServices<IRequestBlocker>();
                foreach (var blocker in blockers)
                {
                    if (!await blocker.AllowRequestAsync(context))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return;
                    }
                }
                await next.Invoke();
            });

        }

    }
}