using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ЛР1
{
    internal class Program
    {
        static string logFile = "game_log.txt";
        static string loginData = "login_Data.txt";
        static void Main(string[] args)
        {
			string username;
			string password;
			Console.WriteLine("Пожалуйста, введите имя пользователя:");
			username = Console.ReadLine();
			Console.WriteLine("Пожалуйста, введите пароль:");
			password = Console.ReadLine();
			StartLogin(username, password);
            while (true)
            {
                int usersDigits;
                int machineDigits = Rnd();
                string machineStr = machineDigits.ToString();
                int[] machineArray = new int[4];
				bool newGame = false;
                int attempts = 0;
                int choice;
                bool flag = true;

                for (int i = 0; i < machineStr.Length; i++)
                {
                    machineArray[i] = int.Parse(machineStr[i].ToString());
                }


				Console.WriteLine("Машина загадала четырёхзначный код. Цифры в одном коде не повторяются.");
                Console.WriteLine(machineDigits);
                while (newGame == false)
                {
                    Console.Write("Ваш вариант:");
                    try
                    {
                        usersDigits = Convert.ToInt32(Console.ReadLine());
                        if (usersDigits.ToString().Length > 4)
                        {
                            Console.WriteLine(("Введено больше четырёх символов!"));
                        }
                        Compare(usersDigits, machineArray, out int isCount, out int posCount);
                        attempts++;
                        Console.WriteLine("Вы угадали цифр: " + isCount);
                        Console.WriteLine("Количество цифр на своих местах: " + posCount);
                        if (isCount == 4 && posCount == 4)
                        {
                            newGame = true;
                            Log(attempts, username);
                            UpdateBestResult(attempts, username);
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
						Console.WriteLine("Пожалуйста, введите имя пользователя:");
						username = Console.ReadLine();
						Console.WriteLine("Пожалуйста, введите пароль:");
						password = Console.ReadLine();
						StartLogin(username, password);
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
                File.AppendAllText(logFile, logText);
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
            if (File.Exists(logFile))
            {
                bestResultLine = File.ReadAllLines(logFile)[0];
                if (int.TryParse(bestResultLine.Split(' ')[3], out int bestAttempts))
                {
                    // Если текущий результат лучше, обновляем файл
                    if (attempts < bestAttempts)
                    {
                        File.WriteAllLines(logFile, new[] { $"Лучший результат: {attempts} попыток - пользователь {username}" });
                        Console.WriteLine("Лучший результат обновлен.");
                    }
                }
            }
            else
            {
                // Если файл не существует, создаем его с текущим результатом
                File.WriteAllLines(logFile, new[] { $"Лучший результат: {attempts} попыток - пользователь {username}" });
		    	File.AppendAllText(logFile, $"Победа достигнута за {attempts} попыток в {DateTime.Now} - пользователь {username}\n");
			}
		}

        public static void DisplayBestResult()
        {
            if (File.Exists(logFile))
            {
                string bestResultLine = File.ReadAllLines(logFile)[0];
                Console.WriteLine(bestResultLine);
            }
            else
            {
                Console.WriteLine("Журнал побед пуст.");
            }
        }
        public static void Compare(int usersDigits, int[] arrMachineDigits, out int isCount, out int posCount)
        {
            string usersStr = usersDigits.ToString();
            int[] usersArray = new int[4];
            try
            {
			    for (int i = 0; i < usersStr.Length; i++)
                {
                    usersArray[i] = int.Parse(usersStr[i].ToString());
                }
            }
            catch (Exception)
            {
				Console.WriteLine("Код состоит ровно из четырёх цифр! Буквы и другие символы недопустимы!");
			
			}
            isCount = 0; //кол-во угаданных цифр
            posCount = 0; //кол-во цифр на своих позициях

            bool allSameDigits = (usersArray[0] == usersArray[1] && usersArray[1] == usersArray[2] && usersArray[2] == usersArray[3] );

            HashSet<int> countedDigits = new HashSet<int>();

            for (int i = 0; i < arrMachineDigits.Length; i++)
            {
                if (arrMachineDigits[i] == usersArray[i])
                {
                    isCount++;
                    posCount++;
                }
                else if (!allSameDigits && arrMachineDigits.Contains(usersArray[i]) && !countedDigits.Contains(usersArray[i]))
                {
                    isCount++;
                    countedDigits.Add(usersArray[i]);
                }
                else if (allSameDigits && arrMachineDigits.Contains(usersArray[0]) && !countedDigits.Contains(usersArray[0]))
                {
                    isCount = 1;
                    posCount = 1;
                    countedDigits.Add(usersArray[0]);
                    break;
                }
            }
        }
        public static void StartLogin(string username, string password)
        { 
			while (Login(username, password) == false)
			{
				System.Console.WriteLine("Имя пользователя или пароль введены неверно. Попробуйте еще раз");
				Console.WriteLine("Пожалуйста, введите имя пользователя:");
				username = Console.ReadLine();
				Console.WriteLine("Пожалуйста, введите пароль:");
				password = Console.ReadLine();
			}
			Console.WriteLine("Авторизация прошла успешно");
		}
        public static bool Login(string username, string password)
        {
            if (File.Exists(loginData))
            {
                for (int i = 0; i < File.ReadAllLines(loginData).Length; i = i+3)
                {
                    if(File.ReadAllLines(loginData)[i].Equals(username) && File.ReadAllLines(loginData)[i+1].Equals(password))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        } 
    }
}
