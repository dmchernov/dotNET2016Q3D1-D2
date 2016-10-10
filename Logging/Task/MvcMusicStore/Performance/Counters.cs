using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PerformanceCounterHelper;

namespace MvcMusicStore.Performance
{
	[PerformanceCounterCategory("MusicStoreCounters", PerformanceCounterCategoryType.MultiInstance, "MusicStoreCounters")]
	public enum Counters
	{
		[PerformanceCounter("LogInCount", "Login's count", PerformanceCounterType.NumberOfItems32)]
		LogIn,
		
		[PerformanceCounter("LogOffCount", "Logoff's count", PerformanceCounterType.NumberOfItems32)]
		LogOff,

		[PerformanceCounter("Items in card", "Items in card count", PerformanceCounterType.NumberOfItems32)]
		ItemsInCard
	}
}