//Приведите пример использования Nullable типа при проектировании класса.
//Класс не используется для работы с записями БД и не используется для их представления.
//Обоснуйте свой выбор.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NullableUsing
{
	class Program
	{
		static void Main(string[] args)
		{
			Example e = new Example();
			Thread t= new Thread(e.Process);
			//e.Process();
			t.Start();
			Thread.Sleep(1000);
			while (!e.Info.IsEnded())
			{
				Console.WriteLine("Процесс не завершен");
				Thread.Sleep(1200);
			}
		}
	}

	class Example
	{
		public ProcessInfo Info;
		public void Process()
		{
			var arr = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
			Info = new ProcessInfo("Перебор массива");
			Console.WriteLine("Начало процесса: {0:G}\nОписание процесса: {1}", Info.StartDate, Info.Data);
			for (int i = 0; i < arr.Length; i++)
			{
				Console.WriteLine(i);
				Thread.Sleep(500);
			}
			Info.EndDate = DateTime.Now;
			Console.WriteLine("Процесс завершен {0:G}", Info.EndDate);
			Console.ReadKey();
		}
	}

	//Класс может быть использован для отслеживания протекания какого-либо процесса.
	//В момент запуска какого-то действия происходит запись информации о событии и время его начала в поля Data и StartDate.
	//Поле EndDate остается неопределенным до тех пор, пока процесс не завершится.
	class ProcessInfo
	{
		public readonly DateTime StartDate;
		private DateTime? endDate;
		public readonly String Data;

		public ProcessInfo(String data)
		{
			StartDate = DateTime.Now;
			Data = data;
		}

		public DateTime? EndDate
		{
			get { return endDate; }
			set
			{
				if (value != null && value.Value > StartDate && !this.IsEnded())
					endDate = value;
				else
					throw new Exception("Введена некорректная дата, или процесс уже завершен");
			}
		}

		/// <summary>
		/// Определяет, имеет ли текущий процесс дату завершения
		/// </summary>
		/// <returns>True, если процесс уже завершен</returns>
		public Boolean IsEnded()
		{
			return endDate.HasValue;
		}
	}
}
