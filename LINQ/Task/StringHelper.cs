using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task
{
	public static class StringHelper
	{
		public static Boolean IsNumber(this String str)
		{
			Int32 number;
			return Int32.TryParse(str,out number);
		}
	}
}
