﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> HttpHeaderAttribute.cs </Name>
//         <Created> 07/06/2017 11:05:39 PM </Created>
//         <Key> 1cda26e2-b361-4a4a-bac7-d7ae718f1dc1 </Key>
//     </File>
//     <Summary>
//         HttpHeaderAttribute.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Puppy.Web.Attributes
{
    /// <summary>
    ///     Require a HTTP header to be specified in a request and/or forwards it in the response. 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class HttpHeaderAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpHeaderAttribute" /> class. 
        /// </summary>
        /// <param name="httpHeaderName"> Name of the HTTP header. </param>
        /// <exception cref="System.ArgumentNullException"> httpHeaderName </exception>
        /// <exception cref="System.ArgumentException">
        ///     Http header name cannot be empty. - httpHeaderName
        /// </exception>
        public HttpHeaderAttribute(string httpHeaderName)
        {
            if (httpHeaderName == null)
                throw new ArgumentNullException(nameof(httpHeaderName));

            if (string.IsNullOrWhiteSpace(httpHeaderName))
                throw new ArgumentException("Http header name cannot be empty.", nameof(httpHeaderName));

            HttpHeaderName = httpHeaderName;
        }

        /// <summary>
        ///     Gets or sets the name of the HTTP header. 
        /// </summary>
        /// <value> The name of the HTTP header. </value>
        public string HttpHeaderName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the specified HTTP header will be taken from
        ///     the request and forwarded to the response.
        /// </summary>
        /// <value> <c> true </c> if the HTTP header is forwarded; otherwise, <c> false </c>. </value>
        public bool Forward { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the specified HTTP header is required. If <c>
        ///     true </c> and the HTTP header is not specified, a 400 Bad Request response will be sent.
        /// </summary>
        /// <value> <c> true </c> if required; otherwise, <c> false </c>. </value>
        public bool Required { get; set; }

        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headerValue = context.HttpContext.Request.Headers
                .Where(x => string.Equals(x.Key, HttpHeaderName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value)
                .FirstOrDefault();

            if (StringValues.IsNullOrEmpty(headerValue))
            {
                if (Required)
                {
                    var message = $"{HttpHeaderName} HTTP header is required.";
                    LogInformation(context, message);
                    context.Result = new BadRequestObjectResult(message);
                }
            }
            else
            {
                if (Forward)
                    context.HttpContext.Response.Headers.Add(HttpHeaderName, headerValue);

                if (Required && !IsValid(headerValue))
                {
                    var message = $"{HttpHeaderName} HTTP header value '{headerValue}' is invalid.";
                    LogInformation(context, message);
                    context.Result = new BadRequestObjectResult(message);
                }
            }
        }

        /// <summary>
        ///     Returns <c> true </c> if the header value is valid, otherwise <c> false </c>. 
        /// </summary>
        /// <param name="headerValues"> The header values. </param>
        /// <returns>
        ///     <c> true </c> if the specified HTTP header values are valid; otherwise, <c> false </c>.
        /// </returns>
        public virtual bool IsValid(StringValues headerValues)
        {
            return true;
        }

        private static void LogInformation(ActionExecutingContext context, string message)
        {
            var factory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = factory.CreateLogger<HttpHeaderAttribute>();
            logger.LogInformation(message);
        }
    }
}