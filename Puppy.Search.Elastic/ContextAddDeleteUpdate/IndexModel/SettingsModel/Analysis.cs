﻿namespace Puppy.Search.Elastic.ContextAddDeleteUpdate.IndexModel.SettingsModel
{
    // "analysis" : {
    //	"filter" : {
    //		"blocks_filter" : {
    //			"type" : "word_delimiter",
    //			"preserve_original": "true"
    //		},
    //	   "shingle":{
    //		   "type":"shingle",
    //		   "max_shingle_size":5,
    //		   "min_shingle_size":2,
    //		   "output_unigrams":"true"
    //		},
    //		"filter_stop":{
    //		   "type":"stop",
    //		   "enable_position_increments":"false"
    //		}
    //	},
    //	"analyzer" : {
    //		"blocks_analyzer" : {
    //			"type" : "custom",
    //			"tokenizer" : "whitespace",
    //			"filter" : ["lowercase", "blocks_filter", "shingle"]
    //		}
    //	}
    //}
    public class Analysis
    {
        public Analysis()
        {
            Filters = new AnalysisFilter();
            Analyzer = new AnalysisAnalyzer();
            Tokenizers = new AnalysisTokenizer();
            CharFilters = new AnalysisCharFilter();
        }

        public AnalysisCharFilter CharFilters { get; set; }
        public AnalysisTokenizer Tokenizers { get; set; }
        public AnalysisFilter Filters { get; set; }
        public AnalysisAnalyzer Analyzer { get; set; }

        public virtual void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            elasticCrudJsonWriter.JsonWriter.WritePropertyName("analysis");
            elasticCrudJsonWriter.JsonWriter.WriteStartObject();

            Tokenizers.WriteJson(elasticCrudJsonWriter);
            Filters.WriteJson(elasticCrudJsonWriter);
            CharFilters.WriteJson(elasticCrudJsonWriter);
            Analyzer.WriteJson(elasticCrudJsonWriter);

            elasticCrudJsonWriter.JsonWriter.WriteEndObject();
        }
    }
}