﻿using System.Net;

namespace TopCore.Framework.Search.Elastic.Model
{
	public class ResultDetails<T>
    {
        public HttpStatusCode Status { get; set; }
        public string RequestBody { get; set; }
        public string RequestUrl { get; set; }
        public string Description { get; set; }
        public T PayloadResult { get; set; }
    }
}