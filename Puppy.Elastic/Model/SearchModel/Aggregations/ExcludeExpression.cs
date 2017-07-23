﻿namespace Puppy.Elastic.Model.SearchModel.Aggregations
{
    public class ExcludeExpression : IncludeExcludeBaseExpression
    {
        public ExcludeExpression(string pattern) : base(pattern, "exclude")
        {
        }
    }
}