using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TictactoeBackend.Api.Models;

namespace TictactoeBackend.Api.Results
{
    public class StandartResult<T> : IHttpActionResult where T : class 
    {
        private JsonContent<T> _data;
        private HttpRequestMessage _request;

        public StandartResult(JsonContent<T> data, HttpRequestMessage request)
        {
            _data = data;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = _data.Result == "1" ? HttpStatusCode.OK: HttpStatusCode.BadRequest,
                Content = new ObjectContent<JsonContent<T>>(_data, new JsonMediaTypeFormatter()),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }
    }
}