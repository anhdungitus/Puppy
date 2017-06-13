﻿namespace Puppy.Web.SEO.OpenGraph.Enums
{
    /// <summary>
    ///     <see cref="OpenGraphAgeGroup" /> extension methods. 
    /// </summary>
    internal static class OpenGraphAgeGroupExtensions
    {
        /// <summary>
        ///     Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphAgeGroup" />. 
        /// </summary>
        /// <param name="openGraphAgeGroup"> The open graph age group. </param>
        /// <returns> The lower-case <see cref="string" /> representation of the <see cref="OpenGraphAgeGroup" />. </returns>
        public static string ToLowercaseString(this OpenGraphAgeGroup openGraphAgeGroup)
        {
            switch (openGraphAgeGroup)
            {
                case OpenGraphAgeGroup.Adult:
                    return "adult";

                case OpenGraphAgeGroup.Kids:
                    return "kids";

                default:
                    return string.Empty;
            }
        }
    }
}