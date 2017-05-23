﻿using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.Model.SearchModel.Aggregations
{
    /// <summary>
    ///     A single-value metrics aggregation that calculates an approximate count of distinct
    ///     values. Values can be extracted either from specific fields in the document or generated
    ///     by a script.
    /// </summary>
    public class CardinalityMetricAggregation : BaseMetricAggregation
    {
        private uint _precisionThreshold;
        private bool _precisionThresholdSet;
        private bool _rehash;
        private bool _rehashSet;

        public CardinalityMetricAggregation(string name, string field) : base("cardinality", name, field)
        {
        }

        /// <summary>
        ///     The precision_threshold options allows to trade memory for accuracy, and defines a
        ///     unique count below which counts are expected to be close to accurate. Above this
        ///     value, counts might become a bit more fuzzy. The maximum supported value is 40000,
        ///     thresholds above this number will have the same effect as a threshold of 40000.
        ///     Default value depends on the number of parent aggregations that multiple create
        ///     buckets (such as terms or histograms).
        /// </summary>
        public uint PrecisionThreshold
        {
            get => _precisionThreshold;
            set
            {
                _precisionThreshold = value;
                _precisionThresholdSet = true;
            }
        }

        /// <summary>
        ///     If you computed a hash on client-side, stored it into your documents and want Elastic
        ///     to use them to compute counts using this hash function without rehashing values, it
        ///     is possible to specify rehash: false. Default value is true. Please note that the
        ///     hash must be indexed as a long when rehash is false.
        /// </summary>
        public bool Rehash
        {
            get => _rehash;
            set
            {
                _rehash = value;
                _rehashSet = true;
            }
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }

        private void WriteValues(ElasticJsonWriter elasticCrudJsonWriter)
        {
            JsonHelper.WriteValue("precision_threshold", _precisionThreshold, elasticCrudJsonWriter,
                _precisionThresholdSet);
            JsonHelper.WriteValue("rehash", _rehash, elasticCrudJsonWriter, _rehashSet);
        }
    }
}