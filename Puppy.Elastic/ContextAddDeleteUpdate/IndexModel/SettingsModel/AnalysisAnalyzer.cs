﻿using Puppy.Elastic.ContextAddDeleteUpdate.IndexModel.SettingsModel.Analyzers;
using System.Collections.Generic;

namespace Puppy.Elastic.ContextAddDeleteUpdate.IndexModel.SettingsModel
{
    public class AnalysisAnalyzer
    {
        private List<AnalyzerBase> _analyzers;
        private bool _analyzersSet;

        public List<AnalyzerBase> Analyzers
        {
            get => _analyzers;
            set
            {
                _analyzers = value;
                _analyzersSet = true;
            }
        }

        public void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            if (_analyzersSet)
            {
                elasticCrudJsonWriter.JsonWriter.WritePropertyName("analyzer");
                elasticCrudJsonWriter.JsonWriter.WriteStartObject();

                foreach (var item in _analyzers)
                    item.WriteJson(elasticCrudJsonWriter);
                elasticCrudJsonWriter.JsonWriter.WriteEndObject();
            }
        }
    }
}