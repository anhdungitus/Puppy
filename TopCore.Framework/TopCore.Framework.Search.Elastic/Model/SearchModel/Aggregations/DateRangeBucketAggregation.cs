using System.Collections.Generic;
using TopCore.Framework.Search.Elastic.Model.SearchModel.Aggregations.RangeParam;
using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.Model.SearchModel.Aggregations
{
    /// <summary>
    ///     A range aggregation that is dedicated for date values. The main difference between this
    ///     aggregation and the normal range aggregation is that the from and to values can be
    ///     expressed in Date Math expressions, and it is also possible to specify a date format by
    ///     which the from and to response fields will be returned. Note that this aggregration
    ///     includes the from value and excludes the to value for each range. http://www.elastic.org/guide/en/elastic/reference/current/search-aggregations-bucket-daterange-aggregation.html
    /// </summary>
    public class DateRangeBucketAggregation : BaseBucketAggregation
    {
        private readonly string _field;
        private readonly string _format;
        private readonly List<RangeAggregationParameter<string>> _ranges;
        private bool _keyed;
        private bool _keyedSet;
        private List<ScriptParameter> _params;
        private bool _paramsSet;

        private string _script;
        private bool _scriptSet;

        public DateRangeBucketAggregation(string name, string field, string format,
            List<RangeAggregationParameter<string>> ranges)
            : base("date_range", name)
        {
            _field = field;
            _format = format;
            _ranges = ranges;
        }

        /// <summary>
        ///     If this value is set, the buckets are returned with id classes. 
        /// </summary>
        public bool Keyed
        {
            get => _keyed;
            set
            {
                _keyed = value;
                _keyedSet = true;
            }
        }

        public string Script
        {
            get => _script;
            set
            {
                _script = value;
                _scriptSet = true;
            }
        }

        public List<ScriptParameter> Params
        {
            get => _params;
            set
            {
                _params = value;
                _paramsSet = true;
            }
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }

        private void WriteValues(ElasticJsonWriter elasticCrudJsonWriter)
        {
            JsonHelper.WriteValue("field", _field, elasticCrudJsonWriter);
            JsonHelper.WriteValue("format", _format, elasticCrudJsonWriter);
            JsonHelper.WriteValue("keyed", _keyed, elasticCrudJsonWriter, _keyedSet);

            elasticCrudJsonWriter.JsonWriter.WritePropertyName("ranges");
            elasticCrudJsonWriter.JsonWriter.WriteStartArray();
            foreach (var rangeAggregationParameter in _ranges)
                rangeAggregationParameter.WriteJson(elasticCrudJsonWriter);
            elasticCrudJsonWriter.JsonWriter.WriteEndArray();

            if (_scriptSet)
            {
                elasticCrudJsonWriter.JsonWriter.WritePropertyName("script");
                elasticCrudJsonWriter.JsonWriter.WriteRawValue("\"" + _script + "\"");
                if (_paramsSet)
                {
                    elasticCrudJsonWriter.JsonWriter.WritePropertyName("params");
                    elasticCrudJsonWriter.JsonWriter.WriteStartObject();

                    foreach (var item in _params)
                    {
                        elasticCrudJsonWriter.JsonWriter.WritePropertyName(item.ParameterName);
                        elasticCrudJsonWriter.JsonWriter.WriteValue(item.ParameterValue);
                    }
                    elasticCrudJsonWriter.JsonWriter.WriteEndObject();
                }
            }
        }
    }
}