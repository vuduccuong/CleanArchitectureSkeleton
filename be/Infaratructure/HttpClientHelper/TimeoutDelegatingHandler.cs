using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infaratructure.HttpClientHelper
{
    internal class TimeoutDelegatingHandler : DelegatingHandler
    {
        private const int MAX_TIME_REQUEST = 30;
        private static TimeSpan DefaultTimeout => TimeSpan.FromSeconds(MAX_TIME_REQUEST);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var context = GetCancellationTokenSource(cancellationToken);
            try
            {
                return await base.SendAsync(request, context?.Token ?? cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException();
            }
        }

        private static CancellationTokenSource GetCancellationTokenSource(CancellationToken cancellationToken)
        {
            var timeout = DefaultTimeout;

            if (timeout == Timeout.InfiniteTimeSpan)
            {
                return null;
            }
            else
            {
                var context = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                context.CancelAfter(timeout);
                return context;
            }
        }
    }
}