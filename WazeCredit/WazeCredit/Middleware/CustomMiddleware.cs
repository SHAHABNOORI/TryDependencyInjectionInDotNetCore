using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WazeCredit.Service.LifeTimeExample;

namespace WazeCredit.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TransientService _transientService;

        public CustomMiddleware(RequestDelegate next, TransientService transientService)
        {
            _next = next;
            _transientService = transientService;
        }

        public async Task InvokeAsync(HttpContext context, 
            ScopedService scopedService, SingletonService singletonService)
        {
            context.Items.Add("CustomMiddlewareTransient", $"Transient Middleware - {_transientService.GetGuid()}");
            context.Items.Add("CustomMiddlewareScoped", $"Scoped Middleware - {scopedService.GetGuid()}");
            context.Items.Add("CustomMiddlewareSingleton", $"Singleton Middleware - {singletonService.GetGuid()}");

            await _next(context);
        }
    }
}