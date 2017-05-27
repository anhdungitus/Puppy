using System.Collections.Generic;
using TopCore.Framework.Search.Elastic.Utils;

namespace TopCore.Framework.Search.Elastic.ContextAddDeleteUpdate.IndexModel.SettingsModel.Analyzers
{
    public class LanguageAnalyzer : BaseStopAnalyzer
    {
        private List<string> _stemExclusion;
        private bool _stemExclusionSet;
        private string _stopwordsPath;
        private bool _stopwordsPathSet;

        /// <summary>
        ///     A set of analyzers aimed at analyzing specific language text. The following types are
        ///     supported: arabic, armenian, basque, brazilian, bulgarian, catalan, chinese, cjk,
        ///                czech, danish, dutch, english, finnish, french, galician, german, greek,
        ///                hindi, hungarian, indonesian, irish, italian, latvian, norwegian, persian,
        ///                portuguese, romanian, russian, sorani, spanish, swedish, turkish, thai.
        ///                Configuring language analyzersedit Stopwordsedit All analyzers support
        ///                setting custom stopwords either internally in the config, or by using an
        ///                external stopwords file by setting stopwords_path. The following analyzers
        ///                support setting custom stem_exclusion list: arabic, armenian, basque,
        ///                catalan, bulgarian, catalan, czech, finnish, dutch, english, finnish,
        ///                french, galician, german, irish, hindi, hungarian, indonesian, italian,
        ///                latvian, norwegian, portuguese, romanian, russian, sorani, spanish,
        ///                swedish, turkish.
        /// </summary>
        /// <param name="name">     name of the analyzer </param>
        /// <param name="analyzer"> required for language analyzers </param>
        public LanguageAnalyzer(string name, string analyzer)
        {
            AnalyzerSet = true;
            Name = name.ToLower();
            Type = analyzer;
        }

        /// <summary>
        ///     stopwords_path A path (either relative to config location, or absolute) to a
        ///     stopwords file configuration.
        /// </summary>
        public string StopwordsPath
        {
            get => _stopwordsPath;
            set
            {
                _stopwordsPath = value;
                _stopwordsPathSet = true;
            }
        }

        /// <summary>
        ///     Excluding words from stemmingedit The stem_exclusion parameter allows you to specify
        ///     an array of lowercase words that should not be stemmed. Internally, this
        ///     functionality is implemented by adding the keyword_marker token filter with the
        ///     keywords set to the value of the stem_exclusion parameter.
        /// </summary>
        public List<string> StemExclusion
        {
            get => _stemExclusion;
            set
            {
                _stemExclusion = value;
                _stemExclusionSet = true;
            }
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteJsonBase(elasticCrudJsonWriter, WriteValues);
        }

        private void WriteValues(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteCommonValues(elasticCrudJsonWriter);
            JsonHelper.WriteValue("stopwords_path", _stopwordsPath, elasticCrudJsonWriter, _stopwordsPathSet);
            JsonHelper.WriteListValue("stem_exclusion", _stemExclusion, elasticCrudJsonWriter, _stemExclusionSet);
        }
    }
}