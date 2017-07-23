﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Puppy.Elastic.Model;
using Puppy.Elastic.Tracing;
using Puppy.Elastic.Utils;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Puppy.Elastic.ContextGet
{
    public class InternalGet
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly HttpClient _client;
        private readonly string _connectionString;
        private readonly ElasticSerializerConfiguration _elasticSerializerConfiguration;
        private readonly ITraceProvider _traceProvider;

        public InternalGet(ITraceProvider traceProvider, CancellationTokenSource cancellationTokenSource,
            ElasticSerializerConfiguration elasticSerializerConfiguration, HttpClient client, string connectionString)
        {
            _traceProvider = traceProvider;
            _cancellationTokenSource = cancellationTokenSource;
            _elasticSerializerConfiguration = elasticSerializerConfiguration;
            _client = client;
            _connectionString = connectionString;
        }

        public GetResult Get(Uri uri)
        {
            var syncExecutor = new SyncExecute(_traceProvider);
            var result = syncExecutor.ExecuteResultDetails(() => GetAsync(uri));

            if (result.Status == HttpStatusCode.NotFound)
            {
                _traceProvider.Trace(TraceEventType.Warning, "ElasticSearchContextGet: HttpStatusCode.NotFound");
                throw new ElasticException("ElasticSearchContextGet: HttpStatusCode.NotFound");
            }
            if (result.Status == HttpStatusCode.BadRequest)
            {
                _traceProvider.Trace(TraceEventType.Warning, "ElasticSearchContextGet: HttpStatusCode.BadRequest");
                throw new ElasticException("ElasticSearchContextGet: HttpStatusCode.BadRequest" + result.Description);
            }

            return result.PayloadResult;
        }

        public async Task<ResultDetails<GetResult>> GetAsync(Uri uri)
        {
            _traceProvider.Trace(TraceEventType.Verbose, "{2}: Request for search: {0}", typeof(GetResult), "GetAsync");
            var resultDetails = new ResultDetails<GetResult>
            {
                Status = HttpStatusCode.InternalServerError
            };

            try
            {
                _traceProvider.Trace(TraceEventType.Verbose, "{1}: Request HTTP GET uri: {0}", uri.AbsoluteUri,
                    "get an object");

                resultDetails.RequestUrl = uri.ToString();
                var response = await _client.GetAsync(uri, _cancellationTokenSource.Token).ConfigureAwait(true);

                resultDetails.Status = response.StatusCode;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _traceProvider.Trace(TraceEventType.Warning, "{2}: GetAsync response status code: {0}, {1}",
                        response.StatusCode, response.ReasonPhrase, "GetAsync");
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorInfo = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                        resultDetails.Description = errorInfo;
                        if (errorInfo.Contains("RoutingMissingException"))
                            throw new ElasticException(
                                "HttpStatusCode.BadRequest: RoutingMissingException, adding the parent Id if this is a child item...");

                        return resultDetails;
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        var errorInfo = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                        resultDetails.Description = errorInfo;
                        return resultDetails;
                    }
                }

                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                _traceProvider.Trace(TraceEventType.Verbose, "{1}: Get Request response: {0}", responseString,
                    "Search");
                var responseObject = JObject.Parse(responseString);
                var ser = new JsonSerializer();

                resultDetails.PayloadResult = responseObject.ToObject<GetResult>(ser);
                return resultDetails;
            }
            catch (OperationCanceledException oex)
            {
                _traceProvider.Trace(TraceEventType.Verbose, oex, "{1}: Get Request OperationCanceledException: {0}",
                    oex.Message, "Search");
                return resultDetails;
            }
        }
    }
}