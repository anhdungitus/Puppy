﻿using System;

namespace Puppy.Web.SEO.OpenGraph.Structs
{
    /// <summary>
    ///     Represents a currency type and amount. 
    /// </summary>
    public class OpenGraphCurrency
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenGraphCurrency" /> class. 
        /// </summary>
        /// <param name="amount">   The actual currency amount. </param>
        /// <param name="currency"> The currency type. </param>
        /// <exception cref="System.ArgumentNullException"> currency is <c> null </c>. </exception>
        public OpenGraphCurrency(double amount, string currency)
        {
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        /// <summary>
        ///     Gets the actual currency amount. 
        /// </summary>
        public double Amount { get; }

        /// <summary>
        ///     Gets the currency type. 
        /// </summary>
        public string Currency { get; }
    }
}