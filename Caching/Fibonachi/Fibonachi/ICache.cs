namespace Fibonachi
{
	public interface ICache<T>
	{
		T Get(T obj);
		void Set(T obj);
	}
}
