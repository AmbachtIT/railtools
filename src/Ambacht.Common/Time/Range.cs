using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ambacht.Common.Time
{
	public class Range : Period
	{
		public Range(DateTime start, DateTime end)
		{
			if(end < start)
			{
				var tmp = start;
				start = end;
				end = tmp;
			}
			this.start = start;
			this.end = end;
		}

		private readonly DateTime start, end;

		public override DateTime StartTime { get { return start; } }
		public override DateTime EndTime { get { return end; } }

		public override string Code
		{
			get
			{
				string c = "";
				if (StartTime == DateTime.MinValue)
				{
					c = $"-{EndTime:yyyyMMdd}";
				}
				else if (EndTime == DateTime.MaxValue)
				{
					c = $"{StartTime:yyyyMMdd}-";
				}
				else
				{
					c = string.Format("{0:yyyyMMdd}-{1:yyyyMMdd}", start, end);
				}
					
				return c;
			}
		}

		public override string DisplayName
		{
			get
			{
				string c = "";
				if (StartTime == DateTime.MinValue)
				{
					c = string.Format("... - {0:yyyy-MM-dd}", end.AddDays(-1));

				}
				else if (EndTime == DateTime.MaxValue)
				{
					c = $"{StartTime:yyyy-MM-dd} - ...";
				}
				else
				{
					c = string.Format("{0:yyyy-MM-dd} - {1:yyyy-MM-dd}", start, end.AddDays(-1));
				}
					
				return c;
			}
		}

        public Period Normalize()
        {
            return CreateYear()
                   ?? CreateQuarter()
                   ?? CreateMonth()
                   ?? CreateWeek()
                   ?? this;
        }

	    private Period CreateYear()
	    {
	        if (StartTime.DayOfYear == 1 && EndTime.DayOfYear == 1)
	        {
	            if (StartTime.Year + 1 == EndTime.Year)
	            {
                    return PeriodType.Year.ByTime(StartTime);
                }
	        }
	        return null;
	    }

        private Period CreateQuarter()
        {
            if (StartTime.Day == 1)
            {
                if (StartTime.Month%3 == 1)
                {
                    if (StartTime.AddMonths(3) == EndTime)
                    {
                        return PeriodType.Quarter.ByTime(StartTime);
                    }
                }
            }
            return null;
        }

        private Period CreateMonth()
        {
            if (StartTime.Day == 1)
            {
                if (StartTime.AddMonths(1) == EndTime)
                {
                    return PeriodType.Month.ByTime(StartTime);
                }
            }
            return null;
        }

        private Period CreateWeek()
        {
            if (StartTime.DayOfWeek == DayOfWeek.Monday)
            {
                if (StartTime.AddDays(7) == EndTime)
                {
                    return PeriodType.Week.ByTime(StartTime);
                }
            }
            return null;
        }
    }

	public class RangePeriodType : PeriodType
	{
		public override Period ByCode(string code)
		{
			if (code != null && code.Length == 17)
			{
				var parts = code.Split('-');
				if(parts.Length == 2)
				{
					var start = Time.Day.Parse(parts[0], true);
					var end = Time.Day.Parse(parts[1], true);
					if(start.HasValue && end.HasValue)
					{
						return new Range(start.Value, end.Value);
					}
				}
			}
			else if (code != null && code.Length == 9)
			{
				if (code.StartsWith("-"))
				{
					//Open Start
					var start = DateTime.MinValue;
					string date = code.Substring(1);
					var end = Time.Day.Parse(date, true);
					
					if(end.HasValue)
					{
						return new Range(start, end.Value);
					}
				}
				else if (code.EndsWith("-"))
				{
					//Open End
					var end = DateTime.MaxValue;
					string date = code.Substring(0, code.Length - 1);
					var start = Time.Day.Parse(date, true);
					
					if(start.HasValue)
					{
						return new Range(start.Value, end);
					}
				}
			}
			return null;
		}

		public override Period ByTime(DateTime time)
		{
			throw new InvalidOperationException();
		}
	}
}
