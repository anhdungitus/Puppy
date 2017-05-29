﻿using Puppy.Search.Elastic.Utils;

namespace Puppy.Search.Elastic.Model.SearchModel.Queries
{
    public class SpanNotQuery : ISpanQuery
    {
        private readonly ISpanQuery _exclude;
        private readonly ISpanQuery _include;
        private uint _dist;
        private bool _distSet;
        private uint _post;
        private bool _postSet;
        private uint _pre;
        private bool _preSet;

        /// <summary>
        ///     Removes matches which overlap with another span query. The span not query maps to
        ///     Lucene SpanNotQuery. The include and exclude clauses can be any span type query. The
        ///     include clause is the span query whose matches are filtered, and the exclude clause
        ///     is the span query whose matches must not overlap those returned. In the above example
        ///     all documents with the term hoya are filtered except the ones that have la preceeding them.
        /// </summary>
        public SpanNotQuery(ISpanQuery include, ISpanQuery exclude)
        {
            _include = include;
            _exclude = exclude;
        }

        /// <summary>
        ///     pre If set the amount of tokens before the include span can’t have overlap with the
        ///     exclude span.
        /// </summary>
        public uint Pre
        {
            get => _pre;
            set
            {
                _pre = value;
                _preSet = true;
            }
        }

        /// <summary>
        ///     post If set the amount of tokens after the include span can’t have overlap with the
        ///     exclude span.
        /// </summary>
        public uint Post
        {
            get => _post;
            set
            {
                _post = value;
                _postSet = true;
            }
        }

        /// <summary>
        ///     dist If set the amount of tokens from within the include span can’t have overlap with
        ///     the exclude span. Equivalent of setting both pre and post.
        /// </summary>
        public uint Dist
        {
            get => _dist;
            set
            {
                _dist = value;
                _distSet = true;
            }
        }

        public void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            elasticCrudJsonWriter.JsonWriter.WritePropertyName("span_not");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();

            elasticCrudJsonWriter.JsonWriter.WritePropertyName("include");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();
            _include.WriteJson(elasticCrudJsonWriter);
            elasticCrudJsonWriter.JsonWriter.WriteEndObject();

            elasticCrudJsonWriter.JsonWriter.WritePropertyName("exclude");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();
            _exclude.WriteJson(elasticCrudJsonWriter);
            elasticCrudJsonWriter.JsonWriter.WriteEndObject();

            if (_distSet)
            {
                JsonHelper.WriteValue("dist", _dist, elasticCrudJsonWriter, _distSet);
            }
            else
            {
                JsonHelper.WriteValue("pre", _pre, elasticCrudJsonWriter, _preSet);
                JsonHelper.WriteValue("post", _post, elasticCrudJsonWriter, _postSet);
            }

            elasticCrudJsonWriter.JsonWriter.WriteEndObject();
        }
    }
}