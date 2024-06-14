using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Ambacht.Common.Time
{
	public abstract class Period : IComparable<Period>
	{

		/// <summary>
		/// Start _time_ of this period (inclusive)
		/// </summary>
		public abstract DateTime StartTime { get; }

		/// <summary>
		/// End _time_ of this period (exclusive)
		/// </summary>
		public abstract DateTime EndTime { get; }

		public DateTime Center {get { return StartTime.AddHours(Duration.TotalHours/2); }}

		public TimeSpan Duration { get { return EndTime.Subtract(StartTime); } }


		public abstract string Code { get; }

		public abstract string DisplayName { get; }

		/// <summary>
		/// Does this period contain the specified time?
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public bool Contains(DateTime time)
		{
			return time >= StartTime && time < EndTime;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Period;
			if(other != null)
			{
				return Code == other.Code;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Code.GetHashCode();
		}

		public static Period ByCode(string code)
		{
			if(string.IsNullOrEmpty(code))
			{
				return null;
			}

			return
				Types()
					.Select(type => type.ByCode(code))
					.SingleOrDefault(p => p != null) 
                    ?? GetRelativePeriod(code)
					?? new CustomPeriod(code);
		}

		/// <summary>
		/// Resolves periods like 'LastWeek', 'ThisYear', etc.
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		private static Period GetRelativePeriod(string code)
		{
			if (code.StartsWith("This") || code.StartsWith("Last"))
			{
				var type = GetTypeByName(code.Substring(4));
				if (type != null)
				{
					var period = type.ByTime(DateTime.Now);
					if (code.StartsWith("Last"))
					{
						period = type.Previous(period);
					}
					return period;
				}
			}
			return null;
		}

        public static IEnumerable<PeriodType> Types()
		{
			yield return PeriodType.Range;
			yield return PeriodType.Year;
			yield return PeriodType.Quarter;
			yield return PeriodType.Month;
			yield return PeriodType.Week;
			yield return PeriodType.Day;
		}

		public static PeriodType GetTypeByName(string name)
		{
			return Types().SingleOrDefault(t => t.GetType().Name.StartsWith(name));
		}

		public int CompareTo(Period other)
		{
			return string.CompareOrdinal(Code, other.Code);
		}

		public override string ToString()
		{
			return DisplayName;
		}


		public static bool operator <(Period p1, Period p2)
		{
			return p1.CompareTo(p2) < 0;
		}


		public static bool operator >(Period p1, Period p2)
		{
			return p1.CompareTo(p2) > 0;
		}

		public static bool operator <=(Period p1, Period p2)
		{
			return p1.CompareTo(p2) <= 0;
		}


		public static bool operator >=(Period p1, Period p2)
		{
			return p1.CompareTo(p2) >= 0;
		}


	}

    /// <summary>
    /// This is a custom period for special cases. Should be handled by calling code.
    /// </summary>
    public class CustomPeriod : Period
    {
        public CustomPeriod(string code)
        {
            this.code = code;
        }

        private readonly string code;

        public override DateTime StartTime
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime EndTime
        {
            get { throw new NotImplementedException(); }
        }

        public override string Code
        {
            get { return code; }
        }

        public override string DisplayName
        {
            get { return Code; }
        }
    }

	/// <summary>
	/// 
	/// </summary>
	public abstract class PeriodType
	{

		/// <summary>
		/// Parses the specified code
		/// </summary>
		/// <param name="code"></param>
		/// <returns>a period, or null of the code does not represent a valid period</returns>
		public abstract Period ByCode(string code);

		/// <summary>
		/// Returns the period containing the specified time
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public abstract Period ByTime(DateTime time);

		/// <summary>
		/// Returns the next period
		/// </summary>
		/// <param name="period"></param>
		/// <returns></returns>
		public Period Next(Period period)
		{
			return ByTime(period.EndTime);
		}

		/// <summary>
		/// Returns the next period
		/// </summary>
		/// <param name="period"></param>
		/// <returns></returns>
		public Period Previous(Period period)
		{
			return ByTime(period.StartTime.AddMinutes(-1));
		}

		/// <summary>
		/// Returns all periods
		/// </summary>
		/// <param name="times"></param>
		/// <returns></returns>
		public IEnumerable<Period> AllPeriods(IEnumerable<DateTime> times)
		{
			var result = new SortedSet<Period>();
			foreach(var time in times)
			{
				result.Add(ByTime(time));
			}
			return result;
		}

		/// <summary>
		/// Returns all periods
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Period> AllConsecutivePeriods(DateTime startDate, DateTime endDate)
		{
			if(startDate > endDate)
			{
				var tmp = startDate;
				startDate = endDate;
				endDate = tmp;
			}
			var startPeriod = ByTime(startDate);
			var endPeriod = ByTime(endDate);
			var period = startPeriod;
			do
			{
				yield return period;
				period = Next(period);
			} while (period.StartTime <= endPeriod.StartTime);
		}

		public IEnumerable<Grouping<T>> Group<T>(IEnumerable<T> items, Func<T, DateTime> getDate)
		{
			var groupings = new Dictionary<Period, Grouping<T>>();
			Period start = null, end = null;
			foreach(var item in items)
			{
				var period = ByTime(getDate(item));
				Grouping<T> grouping;
				if(!groupings.TryGetValue(period, out grouping))
				{
					if (start == null || start > period)
					{
						start = period;
					}
					if (end == null || end < period)
					{
						end = period;
					}
					groupings.Add(period, grouping = new Grouping<T>()
					                      {
					                      	Period = period,
					                      	Items = new List<T>()
					                      });
				}
				grouping.Items.Add(item);
			}
			if(start == null)
			{
				yield break;
			}

			var current = start;
			while(current <= end)
			{
				Grouping<T> grouping;
				if(groupings.TryGetValue(current, out grouping))
				{
					yield return grouping;
				}
				else
				{
					yield return new Grouping<T>()
					             {
					             	Period = current,
									Items = new List<T>()
					             };
				}
				current = Next(current);
			}
		}

		public class Grouping<T>
		{
			public Period Period { get; set; }
			public List<T> Items { get; set; }
		}


		public static readonly PeriodType Range = new RangePeriodType();
		public static readonly PeriodType Year = new YearPeriodType();
		public static readonly PeriodType Quarter = new QuarterPeriodType();
		public static readonly PeriodType Month = new MonthPeriodType();
		public static readonly PeriodType Week = new WeekPeriodType();
		public static readonly PeriodType Day = new DayPeriodType();


		public static PeriodType GetTypeByCode(string code) => All().Single(t => t.Key == code);



		public static IEnumerable<PeriodType> All()
		{
			yield return PeriodType.Day;
			yield return PeriodType.Week;
			yield return PeriodType.Month;
			yield return PeriodType.Quarter;
			yield return PeriodType.Year;
		}

		public static PeriodType SelectPeriodType(DateTime startDate, DateTime endDate, int desiredCount)
        {
            return All()
	            .OrderBy(type => Math.Abs(NeededPeriods(type, startDate, endDate) - desiredCount))
                .First();
        }

        private static int NeededPeriods(PeriodType type, DateTime startDate, DateTime endDate)
        {
            return type.AllConsecutivePeriods(startDate, endDate).Count();
        }


	    public string Key
	    {
	        get { return GetType().Name.Replace("PeriodType", ""); }
	    }

	}

}
