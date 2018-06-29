﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Puppy </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Puppy </Project>
//     <File>
//         <Name> FriendlyUrlHelper.cs </Name>
//         <Created> 07/06/2017 9:52:05 PM </Created>
//         <Key> 2bf6b36d-a333-47eb-a754-74f4693d3937 </Key>
//     </File>
//     <Summary>
//         FriendlyUrlHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System.Text;
using Puppy.Core.StringUtils;

namespace Puppy.Web
{
    /// <summary>
    ///     Helps convert <see cref="string" /> title text to URL friendly <see cref="string" />'s
    ///     that can safely be displayed in a URL.
    /// </summary>
    public static class FriendlyUrlHelper
    {
        /// <summary>
        ///     Converts the specified title so that it is more human and search engine readable e.g.
        ///     http://example.com/product/123/this-is-the-seo-and-human-friendly-product-title. Note
        ///     that the ID of the product is still included in the URL, to avoid having to deal with
        ///     two titles with the same name. Search Engine Optimization (SEO) friendly URL's gives
        ///     your site a boost in search rankings by including keywords in your URL's. They are
        ///     also easier to read by users and can give them an indication of what they are
        ///     clicking on when they look at a URL. Refer to the code example below to see how this
        ///     helper can be used. Go to definition on this method to see a code example. To learn
        ///     more about friendly URL's see
        ///     https://moz.com/blog/15-seo-best-practices-for-structuring-urls. To learn more about
        ///     how this was implemented see http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/25486
        /// </summary>
        /// <param name="title">        The title of the URL. </param>
        /// <param name="remapToAscii">
        ///     if set to <c> true </c>, remaps special UTF8 characters like 'è' to their ASCII
        ///     equivalent 'e'. All modern browsers except Internet Explorer display the 'è'
        ///     correctly. Older browsers and Internet Explorer percent encode these international
        ///     characters so they are displayed as'%C3%A8'. What you set this to depends on whether
        ///     your target users are English speakers or not.
        /// </param>
        /// <param name="maxLength">    The maximum allowed length of the title. </param>
        /// <returns> The SEO and human friendly title. </returns>
        /// <code>
        ///  [HttpGet("product/{id}/{title}", Name = "GetDetails")]
        ///  public IActionResult Product(int id, string title)
        ///  {
        ///      // Get the product as indicated by the ID from a database or some repository.
        ///      var product = ProductRepository.Find(id);
        ///
        ///      // If a product with the specified ID was not found, return a 404 Not Found response.
        ///      if (product == null)
        ///      {
        ///          return this.HttpNotFound();
        ///      }
        ///
        ///      // Get the actual friendly version of the title.
        ///      var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Title);
        ///
        ///      // Compare the title with the friendly title.
        ///      if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
        ///      {
        ///          // If the title is null, empty or does not match the friendly title, return a 301 Permanent
        ///          // Redirect to the correct friendly URL.
        ///          return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
        ///      }
        ///
        ///      // The URL the client has browsed to is correct, show them the view containing the product.
        ///      return this.View(product);
        ///  }
        /// </code>
        public static string GetFriendlyTitle(string title, bool remapToAscii = false, int maxLength = 80)
        {
            if (title == null)
            {
                return string.Empty;
            }

            title = StringHelper.Normalize(title);

            title = title.ToLowerInvariant();


            var length = title.Length;
            var prevDash = false;
            var stringBuilder = new StringBuilder(length);

            for (var i = 0; i < length; ++i)
            {
                var c = title[i];
                if (c >= 'a' && c <= 'z' || c >= '0' && c <= '9')
                {
                    stringBuilder.Append(c);
                    prevDash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevDash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                         c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevDash && stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('-');
                        prevDash = true;
                    }
                }
                else if (c >= 128)
                {
                    var previousLength = stringBuilder.Length;

                    if (remapToAscii)
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    else
                        stringBuilder.Append(c);

                    if (previousLength != stringBuilder.Length)
                        prevDash = false;
                }

                if (stringBuilder.Length >= maxLength)
                    break;
            }

            if (prevDash || stringBuilder.Length > maxLength)
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Remaps the international character to their equivalent ASCII characters. See http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696 
        /// </summary>
        /// <param name="character"> The character to remap to its ASCII equivalent. </param>
        /// <returns> The remapped character </returns>
        private static string RemapInternationalCharToAscii(char character)
        {
            var s = character.ToString().ToLowerInvariant();
            if ("àåáâäãåąā".Contains(s))
                return "a";
            if ("èéêěëę".Contains(s))
                return "e";
            if ("ìíîïı".Contains(s))
                return "i";
            if ("òóôõöøőð".Contains(s))
                return "o";
            if ("ùúûüŭů".Contains(s))
                return "u";
            if ("çćčĉ".Contains(s))
                return "c";
            if ("żźž".Contains(s))
                return "z";
            if ("śşšŝ".Contains(s))
                return "s";
            if ("ñń".Contains(s))
                return "n";
            if ("ýÿ".Contains(s))
                return "y";
            if ("ğĝ".Contains(s))
                return "g";
            if ("ŕř".Contains(s))
                return "r";
            if ("ĺľł".Contains(s))
                return "l";
            if ("úů".Contains(s))
                return "u";
            if ("đď".Contains(s))
                return "d";
            if (character == 'ť')
                return "t";
            if (character == 'ž')
                return "z";
            if (character == 'ß')
                return "ss";
            if (character == 'Þ')
                return "th";
            if (character == 'ĥ')
                return "h";
            if (character == 'ĵ')
                return "j";
            return string.Empty;
        }
    }
}