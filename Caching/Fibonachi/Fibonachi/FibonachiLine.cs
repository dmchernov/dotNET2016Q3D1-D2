using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fibonachi
{
	[Serializable]
	public class FibonachiLine : ICalculate<FibonachiLine>
	{
		[DataMember]
		public List<ulong> Line { get; private set; } = new List<ulong>();
		[DataMember]
		public int Length { get; private set; }

		public FibonachiLine(int count)
		{
			Length = count;
		}

		public FibonachiLine Calculate()
		{
			if (Line.Count == Length) return this;

			for (int i = Line.Count; i < Length; i++)
			{
				if (i < 2) Line.Add(1);
				else Line.Add(Line[i-2] + Line[i-1]);
			}

			return this;
		}

		public FibonachiLine CalculateWithPreviousResult(FibonachiLine previous)
		{
			ShowPreviousResults(previous);
			Line = previous.Line;

			return Calculate();
		}

		private void ShowPreviousResults(FibonachiLine previous)
		{
			Console.WriteLine("Результаты предыдущих вычислений из кэша:");
			foreach (var digit in previous.Line)
			{
				Console.Write($"{digit} ");
			}
			Console.WriteLine();
		}
	}
}
