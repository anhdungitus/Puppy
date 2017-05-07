using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.Model.SearchModel.Aggregations
{
	/// <summary>
	///     A special single bucket aggregation that enables aggregating nested documents. 
	/// </summary>
	public class NestedBucketAggregation : BaseBucketAggregation
    {
        private readonly string _path;

        public NestedBucketAggregation(string name, string path)
            : base("nested", name)
        {
            _path = path;
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }

        private void WriteValues(ElasticJsonWriter elasticCrudJsonWriter)
        {
            JsonHelper.WriteValue("path", _path, elasticCrudJsonWriter);
        }
    }
}