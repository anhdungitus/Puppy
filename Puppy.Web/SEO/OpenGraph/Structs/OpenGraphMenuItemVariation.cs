﻿using System;

namespace Puppy.Web.SEO.OpenGraph.Structs
{
    /// <summary>
    ///     Represents the name and price of a menu item variation. 
    /// </summary>
    public class OpenGraphMenuItemVariation
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenGraphMenuItemVariation" /> class. 
        /// </summary>
        /// <param name="name">  The name. </param>
        /// <param name="price"> The price. </param>
        /// <exception cref="System.ArgumentNullException">
        ///     name or price is <c> null </c>.
        /// </exception>
        public OpenGraphMenuItemVariation(string name, OpenGraphCurrency price)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (price == null)
                throw new ArgumentNullException(nameof(price));

            Name = name;
            Price = price;
        }

        /// <summary>
        ///     Gets the name of the menu item variation. 
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the price of the menu item variation. 
        /// </summary>
        public OpenGraphCurrency Price { get; }
    }
}