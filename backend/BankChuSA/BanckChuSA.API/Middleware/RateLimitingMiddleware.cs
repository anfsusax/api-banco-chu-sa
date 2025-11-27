using System.Collections.Concurrent;

namespace BankChuSA.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimitStore = new();

        private readonly int _maxRequests = 100;
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = GetClientKey(context);
            var rateLimitInfo = _rateLimitStore.GetOrAdd(key, _ => new RateLimitInfo());

            if (rateLimitInfo.IsRateLimited(_maxRequests, _timeWindow))
            {
                _logger.LogWarning("Rate limit exceeded for {ClientKey}", key);
                context.Response.StatusCode = 429;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "Taxa de requisições excedida. Tente novamente mais tarde.",
                        retryAfter = rateLimitInfo.GetRetryAfter(_timeWindow)
                    }));
                return;
            }

            rateLimitInfo.Increment();
            await _next(context);
        }

        private string GetClientKey(HttpContext context)
        {
            // Prioriza usuário autenticado, depois IP
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                return $"user:{context.User.Identity.Name}";
            }
            return $"ip:{context.Connection.RemoteIpAddress}";
        }

        private class RateLimitInfo
        {
            private int _requestCount;
            private DateTime _windowStart;

            public bool IsRateLimited(int maxRequests, TimeSpan timeWindow)
            {
                var now = DateTime.UtcNow;
                if (now - _windowStart > timeWindow)
                {
                    _requestCount = 0;
                    _windowStart = now;
                }

                return _requestCount >= maxRequests;
            }

            public void Increment()
            {
                var now = DateTime.UtcNow;
                if (now - _windowStart > TimeSpan.FromMinutes(1))
                {
                    _requestCount = 0;
                    _windowStart = now;
                }
                Interlocked.Increment(ref _requestCount);
            }

            public int GetRetryAfter(TimeSpan timeWindow)
            {
                var elapsed = DateTime.UtcNow - _windowStart;
                return (int)(timeWindow.TotalSeconds - elapsed.TotalSeconds);
            }
        }
    }
}

