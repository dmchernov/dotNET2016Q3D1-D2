﻿//Напишите библиотеку классов, которая содержит метод преобразования строки в целое число.
//Использовать стандартные методы int.Parse() и int.TryParse() запрещено. Предусмотрите корректную обработку ошибок.

using System;

namespace StringToDigit
{
	public static class StringConvert
	{
		public static int ToDigit(String str)
		{
			int result;
			try
			{
				result = Convert.ToInt32(str);
			}
			catch (FormatException ex)
			{
				throw new InvalidOperationException($"Невозможно выполнить операцию преобразования строки {str} в число", ex);
			}
			catch (OverflowException ex)
			{
				throw new OverflowException($"Число {str} выходит за рамки диапазона Int32", ex);
			}
			return result;
		}
	}
}
