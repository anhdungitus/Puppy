﻿using System;
using System.Collections.Generic;

namespace Puppy.Web.SEO.OpenGraph.Structs
{
    /// <summary>
    ///     A split value in a fitness event. 
    /// </summary>
    public class OpenGraphSplit
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenGraphSplit" /> class. 
        /// </summary>
        /// <param name="isMiles">
        ///     if set to <c> true </c> distance is measured in miles, otherwise kilometres.
        /// </param>
        /// <param name="values">  The values. </param>
        /// <exception cref="System.ArgumentNullException"> values </exception>
        public OpenGraphSplit(bool isMiles, IEnumerable<OpenGraphQuantity> values)
        {
            IsMiles = isMiles;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        /// <summary>
        ///     Gets a value indicating whether distance is measured in miles. 
        /// </summary>
        public bool IsMiles { get; }

        /// <summary>
        ///     Gets a value indicating whether distance is measured in kilometres. 
        /// </summary>
        public bool IsKilometers => !IsMiles;

        /// <summary>
        ///     Gets the split values. 
        /// </summary>
        public IEnumerable<OpenGraphQuantity> Values { get; }
    }
}