﻿using Puppy.Elastic.ContextAddDeleteUpdate.IndexModel;
using Puppy.Elastic.Model;
using Puppy.Elastic.Tracing;
using Puppy.Elastic.Utils;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Puppy.Elastic.ContentExists
{
    public class ElasticContextExists
    {
        private readonly string _connectionString;
        private readonly ElasticSerializerConfiguration _elasticSerializerConfiguration;
        private readonly ITraceProvider _traceProvider;
        public readonly Exists ExistsHeadRequest;

        public ElasticContextExists(ITraceProvider traceProvider, CancellationTokenSource cancellationTokenSource,
            ElasticSerializerConfiguration elasticSerializerConfiguration, HttpClient client, string connectionString)
        {
            _traceProvider = traceProvider;
            _elasticSerializerConfiguration = elasticSerializerConfiguration;
            _connectionString = connectionString;
            ExistsHeadRequest = new Exists(_traceProvider, cancellationTokenSource, client);
        }

        public async Task<ResultDetails<bool>> DocumentExistsAsync<T>(object entityId,
            RoutingDefinition routingDefinition)
        {
            var elasticSearchMapping =
                _elasticSerializerConfiguration.ElasticMappingResolver.GetElasticSearchMapping(typeof(T));
            _traceProvider.Trace(TraceEventType.Verbose,
                "ElasticContextExists: IndexExistsAsync for Type:{0}, Index: {1}, IndexType: {2}, Entity {3}",
                typeof(T),
                elasticSearchMapping.GetIndexForType(typeof(T)),
                elasticSearchMapping.GetDocumentType(typeof(T)),
                entityId
            );

            var elasticUrlForHeadRequest = string.Format("{0}/{1}/{2}/", _connectionString,
                elasticSearchMapping.GetIndexForType(typeof(T)), elasticSearchMapping.GetDocumentType(typeof(T)));

            var uri = new Uri(elasticUrlForHeadRequest + entityId + RoutingDefinition.GetRoutingUrl(routingDefinition));
            return await ExistsHeadRequest.ExistsAsync(uri);
        }

        public async Task<ResultDetails<bool>> IndexExistsAsync<T>()
        {
            var elasticSearchMapping =
                _elasticSerializerConfiguration.ElasticMappingResolver.GetElasticSearchMapping(typeof(T));
            _traceProvider.Trace(TraceEventType.Verbose,
                "ElasticContextExists: IndexExistsAsync for Type:{0}, Index: {1}",
                typeof(T),
                elasticSearchMapping.GetIndexForType(typeof(T))
            );

            var elasticUrlForHeadRequest = string.Format("{0}/{1}", _connectionString,
                elasticSearchMapping.GetIndexForType(typeof(T)));

            var uri = new Uri(elasticUrlForHeadRequest);
            return await ExistsHeadRequest.ExistsAsync(uri);
        }

        public async Task<ResultDetails<bool>> IndexTypeExistsAsync<T>()
        {
            var elasticSearchMapping =
                _elasticSerializerConfiguration.ElasticMappingResolver.GetElasticSearchMapping(typeof(T));
            _traceProvider.Trace(TraceEventType.Verbose,
                "ElasticContextExists: IndexExistsAsync for Type:{0}, Index: {1}, IndexType: {2}",
                typeof(T),
                elasticSearchMapping.GetIndexForType(typeof(T)),
                elasticSearchMapping.GetDocumentType(typeof(T))
            );

            var elasticUrlForHeadRequest = string.Format("{0}/{1}/{2}", _connectionString,
                elasticSearchMapping.GetIndexForType(typeof(T)), elasticSearchMapping.GetDocumentType(typeof(T)));

            var uri = new Uri(elasticUrlForHeadRequest);
            return await ExistsHeadRequest.ExistsAsync(uri);
        }

        public async Task<ResultDetails<bool>> AliasExistsForIndexAsync<T>(string alias)
        {
            var elasticSearchMapping =
                _elasticSerializerConfiguration.ElasticMappingResolver.GetElasticSearchMapping(typeof(T));
            _traceProvider.Trace(TraceEventType.Verbose,
                "ElasticContextExists: AliasExistsAsync for Type:{0}, Index: {1}",
                typeof(T),
                elasticSearchMapping.GetIndexForType(typeof(T))
            );

            var elasticUrlForHeadRequest = string.Format("{0}/{1}/_alias/{2}", _connectionString,
                elasticSearchMapping.GetIndexForType(typeof(T)), alias);

            var uri = new Uri(elasticUrlForHeadRequest);
            return await ExistsHeadRequest.ExistsAsync(uri);
        }

        public async Task<ResultDetails<bool>> AliasExistsAsync(string alias)
        {
            _traceProvider.Trace(TraceEventType.Verbose, "ElasticContextExists: AliasExistsAsync for alias:{0}", alias);

            var elasticUrlForHeadRequest = string.Format("{0}/_alias/{1}", _connectionString, alias);

            var uri = new Uri(elasticUrlForHeadRequest);
            return await ExistsHeadRequest.ExistsAsync(uri);
        }

        public bool Exists(Task<ResultDetails<bool>> method)
        {
            var syncExecutor = new SyncExecute(_traceProvider);
            return syncExecutor.Execute(() => method);
        }

        public async Task<ResultDetails<bool>> ExistsAsync(Uri uri)
        {
            return await ExistsHeadRequest.ExistsAsync(uri);
        }
    }
}