﻿namespace Puppy.Search.Elastic.ContextAlias.AliasModel
{
    // "actions" : [ { "remove" : { "index" : "test1", "alias" : "alias1" } }, { "add" : { "index" :
    // "test1", "alias" : "alias2" } } ]

    public class AliasRemoveParameters : AliasBaseParameters
    {
        public AliasRemoveParameters(string alias, string index) : base(alias, index)
        {
        }

        public override void WriteJson(ElasticJsonWriter elasticCrudJsonWriter)
        {
            WriteInternalJson(elasticCrudJsonWriter, AliasAction.remove);
        }
    }
}