//Приведите пример использования Nullable типа при проектировании класса.
//Класс не используется для работы с записями БД и не используется для их представления.
//Обоснуйте свой выбор.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullableUsing
{
	class Program
	{
		static void Main(string[] args)
		{
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
			EndDate = null;
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
