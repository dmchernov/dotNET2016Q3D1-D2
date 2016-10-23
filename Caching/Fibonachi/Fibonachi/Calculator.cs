namespace Fibonachi
{
	class Calculator<T> where T:ICalculate<T>
	{
		private readonly ICache<T> _cache;

		public Calculator(ICache<T> cache)
		{
			_cache = cache;
		}

		public T Calculate(T arg)
		{
			var cacheResults = _cache.Get(arg);

			if (cacheResults == null)
			{
				var result = arg.Calculate();
				_cache.Set(result);
				return result;
			}

			var newResult = arg.CalculateWithPreviousResult(cacheResults);
			_cache.Set(newResult);
			return newResult;
		}
	}
}