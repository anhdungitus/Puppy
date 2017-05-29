using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Puppy.Search.Elastic.ContextSearch.SearchModel.AggModel
{
    public class FilterBucketAggregationsResult : AggregationResult<FilterBucketAggregationsResult>
    {
        [JsonProperty("doc_count")]
        public int DocCount { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> SubAggregations { get; set; }

        public T GetSubAggregationsFromJTokenName<T>(string name)
        {
            return SubAggregations[name].ToObject<T>();
        }

        public T GetSingleMetricSubAggregationValue<T>(string name)
        {
            return SubAggregations[name]["value"].Value<T>();
        }

        public override FilterBucketAggregationsResult GetValueFromJToken(JToken result)
        {
            return result.ToObject<FilterBucketAggregationsResult>();
        }
    }
}