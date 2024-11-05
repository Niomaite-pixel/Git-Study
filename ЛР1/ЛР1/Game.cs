using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
	internal class Game
	{
		static string LogFile = "game_log.txt";
		/// <summary>
		/// Запуск игры
		/// </summary>
		public void GameStart()
		{
			User.LoginStart();
			while (true)
			{
				int[] usersDigits;
				int[] machineDigits = Rnd();
				bool newGame = false;
				int attempts = 0;
				int choice;
				bool flag = true;

				Console.WriteLine("Машина загадала четырёхзначный код. Цифры в одном коде не повторяются. Чтобы выйти из приложения, нажмите клавишу Esc.");
				foreach(int digit in machineDigits) {
					Console.Write(digit);
				}
                Console.WriteLine("\n");
				while (newGame == false)
				{
					Console.Write("Ваш вариант:");
					try
					{
						User.TryReadLine(out string _usersDigits);
						usersDigits = _usersDigits.Select(c => (int)char.GetNumericValue(c)).ToArray();
						if (_usersDigits.Length > 4)
						{
							Console.WriteLine(("Введено больше четырёх цифр!"));
						}
						else if (_usersDigits.Length < 4)
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
							UpdateBestResult(attempts, User.Username);
							Log(attempts, User.Username);
						}
					}
					catch (FormatException)
					{
						Console.WriteLine("Буквы и другие символы в коде недопустимы!");
					}
				}
				while (flag == true)
				{
					Console.WriteLine("Желаете продолжить? \n1.Да \n2.Нет \n3.Посмотреть наилучший результат \n4.Сменить пользователя \n5.Добавить нового пользователя");
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
						DisplayBestResult(); 
						continue;
					}
					else if (choice == 4)
					{
						User.LoginStart();
						continue;
					}
					else if (choice == 5)
					{
						User.RegistrationStart();
						continue;
					}
					else
					{
						Console.WriteLine("Некорректный ввод, попробуйте снова.");
					}
				}
			}
		}
		/// <summary>
		/// Генерирует код из 4 цифр, которые не повторяются
		/// </summary>
		/// <returns>4-разрядный код без повторяющихся цифр</returns>
		static int[] Rnd()
		{
			int[] digits = new int[4];
			Random rnd = new Random();
			HashSet<int> usedDigits = new HashSet<int>();

			while (usedDigits.Count < 4)
			{
				int digit = rnd.Next(0, 10); // Генерируем цифру от 0 до 9
				if (usedDigits.Add(digit)) // Проверяем, уникальна ли цифра
				{
					digits[usedDigits.Count - 1] = digit;
				}
			}

			return digits;
		}
		/// <summary>
		/// Записывает информацию о текущей победе файл "game_log"
		/// </summary>
		static void Log(int attempts, string username)
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
		/// <summary>
		/// Обновляет лучший результат в файле "game_log"
		/// </summary>
		static void UpdateBestResult(int attempts, string username)
		{
			string bestResultLine;
			if (File.Exists(LogFile))
			{
				bestResultLine = File.ReadAllLines(LogFile)[0];
				bool yes = int.TryParse(bestResultLine.Split(' ')[2], out int bestAttempts);
				if (yes)
				{					
					if (attempts < bestAttempts) // Если текущий результат лучше, обновляем файл
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
			}
		}
		/// <summary>
		/// Выводит на экран запись из файла "game_log", содержащую информацию о победе за наименьшее количество ходов (количество ходов и имя пользователя) 
		/// </summary>
		static void DisplayBestResult()
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
		/// <summary>
		/// Сравнивает сгенерированный программой код и код, предложенный пользователем
		/// </summary>
		/// <param name="usersDigits">Вариант пользователя в виде целого числа</param>
		static void Compare(int[] usersDigits, int[] machineDigits, out int countIs, out int countPosition)
		{
			HashSet<int> countedDigits = new HashSet<int>();
			countIs = 0; //кол-во угаданных цифр
			countPosition = 0; //кол-во цифр на своих позициях
			try
			{
				bool allSameDigits = (usersDigits[0] == usersDigits[1] && usersDigits[1] == usersDigits[2] && usersDigits[2] == usersDigits[3]);
				for (int i = 0; i < machineDigits.Length; i++)
				{
					if (machineDigits[i] == usersDigits[i])
					{
						countIs++;
						countPosition++;
					}
					else if (!allSameDigits && machineDigits.Contains(usersDigits[i]) && !countedDigits.Contains(usersDigits[i]))
					{
						countIs++;
						countedDigits.Add(usersDigits[i]);
					}
					else if (allSameDigits && machineDigits.Contains(usersDigits[0]) && !countedDigits.Contains(usersDigits[0]))
					{
						countIs = 1;
						countPosition = 1;
						countedDigits.Add(usersDigits[0]);
						break;
					}
				}
			}
			catch (IndexOutOfRangeException) 
			{
                Console.WriteLine("Перепроверьте количество цифр в коде!");
			}
		}
		/// <summary>
		/// Преобразует строку в массив из целых чисел
		/// </summary>
		static void StringToArray(string _string, out int[] _array)
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
