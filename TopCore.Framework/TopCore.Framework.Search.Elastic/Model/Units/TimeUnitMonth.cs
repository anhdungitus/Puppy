﻿namespace TopCore.Framework.Search.Elastic.Model.Units
{
	public class TimeUnitMonth : TimeUnit
    {
        public TimeUnitMonth(int months)
        {
            Months = months;
        }

        public int Months { get; set; }

        public override string GetTimeUnit()
        {
            return Months + "M";
        }
    }
}