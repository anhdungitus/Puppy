﻿namespace Puppy.Elastic.Model.SearchModel.Aggregations
{
    public class IncludeExpression : IncludeExcludeBaseExpression
    {
        public IncludeExpression(string pattern) : base(pattern, "include")
        {
        }
    }
}