using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infaratructure.HttpClientHelper
{
    public class HttpClientRequestIdDelegatingHandler : DelegatingHandler
    {
        private const string REQUEST_ID_HEADER = "x-requestid";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (request != null && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put) && !request.Headers.Contains(REQUEST_ID_HEADER))
            {
                request.Headers.Add(REQUEST_ID_HEADER, Guid.NewGuid().ToString());
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
