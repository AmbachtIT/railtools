using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Ambacht.Common.Time
{

    public class DefaultPeriodService : IPeriodService
    {

		public DefaultPeriodService(INowService nowService)
		{
			_nowService = nowService;
		}

		private readonly INowService _nowService;



		public Period ByCode(string code, DateTime? now = null) => code switch
        {
	        null => null,

	        var s when s.StartsWith("This") => GetThis(s.Substring(4)),
	        var s when s.StartsWith("Previous") => GetPrevious(s.Substring(8)),


			_ => Period.ByCode(code)
        };

        private Period GetThis(string code, DateTime? now = null)
        {
	        var type = PeriodType.GetTypeByCode(code);
	        return type.ByTime(now ?? _nowService.Now);
        }


        private Period GetPrevious(string code, DateTime? now = null)
        {
	        var type = PeriodType.GetTypeByCode(code);
	        return type.Previous(type.ByTime(now ?? _nowService.Now));
        }

	}

	public interface IPeriodService
    {

        Period ByCode(string code, DateTime? now = null);

    }
}
