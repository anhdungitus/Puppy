﻿using TopCore.Framework.Search.Elastic.Model;

namespace TopCore.Framework.Search.Elastic.ContextAddDeleteUpdate.IndexModel.SettingsModel.Tokenizers
{
    public class NGramTokenizer : BaseNGramTokenizer
    {
        /// <summary>
        ///   A tokenizer of type nGram. 
        /// </summary>
        /// <param name="name"> name of the custom tokenizer (ToLower() </param>
        public NGramTokenizer(string name)
        {
            AnalyzerSet = true;
            Name = name.ToLower();
            Type = DefaultTokenizers.NGram;
        }
    }
}