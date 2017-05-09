﻿using System;
using TopCore.Framework.Search.Elastic.ContextAddDeleteUpdate.IndexModel;

namespace TopCore.Framework.Search.Elastic.Model
{
    public class EntityContextInfo
    {
        public EntityContextInfo()
        {
            RoutingDefinition = new RoutingDefinition();
        }

        public object Id { get; set; }
        public bool DeleteDocument { get; set; }
        public Type EntityType { get; set; }
        public Type ParentEntityType { get; set; }
        public RoutingDefinition RoutingDefinition { get; set; }
        public object Document { get; set; }
    }
}