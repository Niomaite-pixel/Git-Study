using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
	internal class Game
	{
		static string LogFile = "game_log.txt";
		public void GameStart()
		{
			while (true)
			{
				int usersDigits;
				int machineDigits = Rnd();
				bool newGame = false;
				int attempts = 0;
				int choice;
				bool flag = true;

				User.LoginStart();

				Console.WriteLine("Машина загадала четырёхзначный код. Цифры в одном коде не повторяются. Чтобы выйти из приложения, нажмите клавишу Esc.");
				Console.WriteLine(machineDigits);
				while (newGame == false)
				{
					Console.Write("Ваш вариант:");
					try
					{
						User.TryReadLine(out string _usersDigits);

						usersDigits = Convert.ToInt32(_usersDigits);
						if (usersDigits.ToString().Length > 4)
						{
							Console.WriteLine(("Введено больше четырёх цифр!"));
						}
						else if (usersDigits.ToString().Length < 4)
						{
							Console.WriteLine(("Введено меньше четырёх цифр!"));
						}
						Compare(usersDigits, machineDigits, out int countIs, out int countPosition);
						attempts++;
						Console.WriteLine("Вы угадали цифр: " + countIs);
						Console.WriteLine("Количество цифр на своих местах: " + countPosition);
						if (countIs == 4 && countPosition == 4)
						{
							newGame = true;
                            Console.Write("Вы победили за количество ходов - " + attempts + "! ");
							Log(attempts, User.Username);
							UpdateBestResult(attempts, User.Username);
						}
					}
					catch (FormatException)
					{
						Console.WriteLine("Буквы и другие символы в коде недопустимы!");
					}
				}
				while (flag == true)
				{
					Console.WriteLine("Желаете продолжить? \n1.Да \n2.Нет \n3.Посмотреть наилучший результат \n4.Сменить пользователя");
					choice = Convert.ToInt32(Console.ReadLine());
					if (choice == 1)
					{
						flag = false;
						continue;
					}
					else if (choice == 2)
					{
						Console.WriteLine("Выход из программы...");
						Environment.Exit(0);
					}
					else if (choice == 3)
					{
						DisplayBestResult(); // Отображение лучшего результата
						continue;
					}
					else if (choice == 4)
					{
						User.LoginStart();
					}
					else
					{
						Console.WriteLine("Некорректный ввод, попробуйте снова.");
					}
				}
			}
		}
		public static int Rnd()
		{
			int[] digits = new int[4];
			int res = 0;
			Random rnd = new Random();
			HashSet<int> usedDigits = new HashSet<int>();

			for (int i = 0; i < 4; i++)
			{
				int digit;
				do
				{
					digit = rnd.Next(0, 10);
				} while (usedDigits.Contains(digit));

				digits[i] = digit;
				usedDigits.Add(digit);
			}

			foreach (int dig in digits)
			{
				res = res * 10 + dig;
			}
			return res;
		}

		public static void Log(int attempts, string username)
		{
			string logText = $"Победа достигнута за {attempts} попыток в {DateTime.Now} - пользователь {username}\n";

			try
			{
				File.AppendAllText(LogFile, logText);
				Console.WriteLine("Результат записан в журнал.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка при записи в журнал: " + ex.Message);
			}
		}

		public static void UpdateBestResult(int attempts, string username)
		{
			string bestResultLine;
			if (File.Exists(LogFile))
			{
				bestResultLine = File.ReadAllLines(LogFile)[0];
				if (int.TryParse(bestResultLine.Split(' ')[3], out int bestAttempts))
				{
					// Если текущий результат лучше, обновляем файл
					if (attempts < bestAttempts)
					{
						File.WriteAllLines(LogFile, new[] { $"Лучший результат: {attempts} попыток - пользователь {username}" });
						Console.WriteLine("Лучший результат обновлен.");
					}
				}
			}
			else
			{
				// Если файл не существует, создаем его с текущим результатом
				File.WriteAllLines(LogFile, new[] { $"Лучший результат: {attempts} попыток - пользователь {username}" });
				File.AppendAllText(LogFile, $"Победа достигнута за {attempts} попыток в {DateTime.Now} - пользователь {username}\n");
			}
		}

		public static void DisplayBestResult()
		{
			if (File.Exists(LogFile))
			{
				string _bestResultLine = File.ReadAllLines(LogFile)[0];
				Console.WriteLine(_bestResultLine);
			}
			else
			{
				Console.WriteLine("Журнал побед пуст.");
				using (StreamWriter writer = new StreamWriter(LogFile))
				{

				}
			}
		}
		public static void Compare(int usersDigits, int machineDigits, out int countIs, out int countPosition)
		{
			string _usersString = usersDigits.ToString();
			string _machineString = machineDigits.ToString();
			StringToArray(_usersString, out int[] usersArray);
			StringToArray(_machineString, out int[] machineArray);
			HashSet<int> countedDigits = new HashSet<int>();
			countIs = 0; //кол-во угаданных цифр
			countPosition = 0; //кол-во цифр на своих позициях
			try
			{
				bool allSameDigits = (usersArray[0] == usersArray[1] && usersArray[1] == usersArray[2] && usersArray[2] == usersArray[3]);
				for (int i = 0; i < machineArray.Length; i++)
				{
					if (machineArray[i] == usersArray[i])
					{
						countIs++;
						countPosition++;
					}
					else if (!allSameDigits && machineArray.Contains(usersArray[i]) && !countedDigits.Contains(usersArray[i]))
					{
						countIs++;
						countedDigits.Add(usersArray[i]);
					}
					else if (allSameDigits && machineArray.Contains(usersArray[0]) && !countedDigits.Contains(usersArray[0]))
					{
						countIs = 1;
						countPosition = 1;
						countedDigits.Add(usersArray[0]);
						break;
					}
				}
			}
			catch (IndexOutOfRangeException) 
			{

			}

		}
		public static void StringToArray(string _string, out int[] _array)
		{
			_array = new int[_string.Length];
			try
			{
				for (int i = 0; i < _string.Length; i++)
				{
					_array[i] = int.Parse(_string[i].ToString());
				}
			}
			catch (IndexOutOfRangeException)
			{
				Console.WriteLine("Код состоит ровно из четырёх цифр!");

			}
		}
	}
}
