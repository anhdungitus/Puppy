using System.Collections.Generic;
using TopCore.Framework.Search.Elastic.Model.Units;
using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.Model.SearchModel.Aggregations
{
    /// <summary>
    ///   A multi-bucket aggregation similar to the histogram except it can only be applied on date values. Since dates are represented in elastic
    ///   internally as long values, it is possible to use the normal histogram on dates as well, though accuracy will be compromised. The reason for
    ///   this is in the fact that time based intervals are not fixed (think of leap years and on the number of days in a month). For this reason, we
    ///   need special support for time based data. From a functionality perspective, this histogram supports the same features as the normal
    ///   histogram. The main difference is that the interval can be specified by date/time expressions
    /// </summary>
    public class DateHistogramBucketAggregation : BaseBucketAggregation
    {
        private readonly string _field;

        private readonly TimeUnit _interval;

        //private OrderAgg _order;
        //private bool _orderSet;
        private ExtendedBounds _extendedBounds;

        private bool _extendedBoundsSet;
        private string _format;
        private bool _formatSet;
        private uint _minDocCount;
        private bool _minDocCountSet;
        private string _offset;
        private bool _offsetSet;
        private List<ScriptParameter> _params;
        private bool _paramsSet;
        private bool _preZoneAdjustLargeInterval;
        private bool _preZoneAdjustLargeIntervalSet;
        private string _script;
        private bool _scriptSet;
        private string _timeZone;
        private bool _timeZoneSet;

        public DateHistogramBucketAggregation(string name, string field, TimeUnit interval) : base("date_histogram",
            name)
        {
            _field = field;
            _interval = interval;
        }

        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                _formatSet = true;
            }
        }

        //public OrderAgg Order
        //{
        //	get { return _order; }
        //	set
        //	{
        //		_order = value;
        //		_orderSet = true;
        //	}
        //}

        /// <summary>
        ///   min_doc_count Terms are collected and ordered on a shard level and merged with the terms collected from other shards in a second step.
        ///   However, the shard does not have the information about the global document count available. The decision if a term is added to a
        ///   candidate list depends only on the order computed on the shard using local shard frequencies. The min_doc_count criterion is only applied
        ///   after merging local terms statistics of all shards. In a way the decision to add the term as a candidate is made without being very
        ///   certain about if the term will actually reach the required min_doc_count. This might cause many (globally) high frequent terms to be
        ///   missing in the final result if low frequent terms populated the candidate lists. To avoid this, the shard_size parameter can be increased
        ///   to allow more candidate terms on the shards. However, this increases memory consumption and network traffic.
        /// </summary>
        public uint MinDocCount
        {
            get => _minDocCount;
            set
            {
                _minDocCount = value;
                _minDocCountSet = true;
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

        public ExtendedBounds ExtendedBounds
        {
            get => _extendedBounds;
            set
            {
                _extendedBounds = value;
                _extendedBoundsSet = true;
            }
        }

        /// <summary>
        ///   offset offset for pre rounding, and post_offset for post rounding. The format is the date time format (1h, 1d, etc�). 
        /// </summary>
        public string Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                _offsetSet = true;
            }
        }

        /// <summary>
        ///   pre_zone_adjust_large_interval Sometimes, we want to apply the same conversion to UTC we did above for hour also for day (and up)
        ///   intervals. We can set pre_zone_adjust_large_interval to true, which will apply the same conversion done for hour interval in the example,
        ///   to day and above intervals (it can be set regardless of the interval, but only kick in when using day and higher intervals).
        /// </summary>
        public bool PreZoneAdjustLargeInterval
        {
            get => _preZoneAdjustLargeInterval;
            set
            {
                _preZoneAdjustLargeInterval = value;
                _preZoneAdjustLargeIntervalSet = true;
            }
        }

        /// <summary>
        ///   time_zone 
        /// </summary>
        public string TimeZone
        {
            get => _timeZone;
            set
            {
                _timeZone = value;
                _timeZoneSet = true;
            }
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }

        private void WriteValues(ElasticJsonWriter elasticCrudJsonWriter)
        {
            JsonHelper.WriteValue("field", _field, elasticCrudJsonWriter);
            JsonHelper.WriteValue("interval", _interval.GetTimeUnit(), elasticCrudJsonWriter);
            JsonHelper.WriteValue("format", _format, elasticCrudJsonWriter, _formatSet);
            JsonHelper.WriteValue("offset", _offset, elasticCrudJsonWriter, _offsetSet);
            JsonHelper.WriteValue("pre_zone_adjust_large_interval", _preZoneAdjustLargeInterval, elasticCrudJsonWriter,
                _preZoneAdjustLargeIntervalSet);
            JsonHelper.WriteValue("time_zone", _timeZone, elasticCrudJsonWriter, _timeZoneSet);

            //if (_orderSet)
            //{
            //	_order.WriteJson(elasticCrudJsonWriter);
            //}
            JsonHelper.WriteValue("min_doc_count", _minDocCount, elasticCrudJsonWriter, _minDocCountSet);

            if (_extendedBoundsSet)
                _extendedBounds.WriteJson(elasticCrudJsonWriter);

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