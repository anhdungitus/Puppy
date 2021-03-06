﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> Exception.cs </Name>
//         <Created> 10/08/17 5:58:52 PM </Created>
//         <Key> 69f64980-151f-44db-ba2a-d05775526df7 </Key>
//     </File>
//     <Summary>
//         Exception.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Newtonsoft.Json;
using System;

namespace Puppy.Core.Models
{
    [Serializable]
    public class ExceptionInfo : SerializableModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string HelpLink { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string TypeName { get; set; }

        public string BaseTypeName { get; set; }

        public ExceptionInfo InternalException { get; set; }

        [JsonIgnore]
        public string RootExceptionMessage => InternalException != null ? InternalException.RootExceptionMessage : Message;

        public ExceptionInfo()
        {
        }

        public ExceptionInfo(string message) : this()
        {
            Message = message;
        }

        public ExceptionInfo(Exception ex) : this(ex?.Message)
        {
            if (ex == null)
            {
                return;
            }

            HelpLink = ex.HelpLink;
            Source = ex.Source;
            StackTrace = ex.StackTrace;
            TypeName = ex.GetType()?.FullName;
            BaseTypeName = ex.GetBaseException()?.GetType()?.FullName;
            InternalException = ex.InnerException != null ? new ExceptionInfo(ex.InnerException) : null;
        }
    }
}