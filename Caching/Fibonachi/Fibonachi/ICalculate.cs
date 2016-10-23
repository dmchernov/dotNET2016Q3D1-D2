namespace Fibonachi
{
	public interface ICalculate<T>
	{
		T Calculate();

		T CalculateWithPreviousResult(T previous);
	}
}
