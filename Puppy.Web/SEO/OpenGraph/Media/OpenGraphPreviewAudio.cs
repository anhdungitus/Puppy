﻿using System.Text;

namespace Puppy.Web.SEO.OpenGraph.Media
{
    /// <summary>
    ///     A audio file that complements this object. Only to be used with <see cref="OpenGraphMusicSong" />. 
    /// </summary>
    public class OpenGraphPreviewAudio : OpenGraphMedia
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenGraphPreviewAudio" /> class. 
        /// </summary>
        /// <param name="audioUrl"> The audio URL. </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if audioUrl is <c> null </c>.
        /// </exception>
        public OpenGraphPreviewAudio(string audioUrl)
            : base(audioUrl)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenGraphPreviewAudio" /> class. 
        /// </summary>
        /// <param name="mediaUrl"> The media URL. </param>
        /// <param name="type">    
        ///     The MIME type of the media e.g. media/ogg. This is optional if your media URL ends
        ///     with a file extension, otherwise it is recommended.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown if <paramref name="mediaUrl" /> is <c> null </c>.
        /// </exception>
        public OpenGraphPreviewAudio(string mediaUrl, string type)
            : this(mediaUrl)
        {
            Type = type;
        }

        /// <summary>
        ///     Appends a HTML-encoded string representing this instance to the
        ///     <paramref name="stringBuilder" /> containing the Open Graph meta tags.
        /// </summary>
        /// <param name="stringBuilder"> The string builder. </param>
        public override void ToString(StringBuilder stringBuilder)
        {
            stringBuilder.AppendMetaPropertyContent("music:preview_url:url", Url);
            stringBuilder.AppendMetaPropertyContentIfNotNull("music:preview_url:secure_url", UrlSecure);
            stringBuilder.AppendMetaPropertyContentIfNotNull("music:preview_url:type", Type);
        }
    }
}