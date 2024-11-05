using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
	internal class User
	{
		public static string Username { get; private set; }
		public static string Password { get; private set; }
		static string LoginData = "login_data.txt";
		static string _username;
		static string _password;
		/// <summary>
		/// Запуск процесса авторищации
		/// </summary>
		public static void LoginStart()
		{
			if (File.Exists(LoginData))
			{
				Console.WriteLine("Перед началом игры необходимо пройти авторизацию. Чтобы выйти из приложения, нажмите клавишу Esc.\nПожалуйста, введите имя пользователя:");
				TryReadLine(out _username);
				Console.WriteLine("Пожалуйста, введите пароль:");
				TryReadLine(out _password);
				LoginCheck(_username, _password);
			}
			else
			{
				RegistrationStart();
			}
		}
		/// <summary>
		/// Проверка прохождения авторизации
		/// </summary>
		static void LoginCheck(string _username, string _password)
		{
			while (LoginSearchUser(_username, _password) == false)
			{
				Console.WriteLine("Имя пользователя или пароль введены неверно. Попробуйте еще раз. Чтобы выйти из приложений, нажмите клавишу Esc");
				Console.WriteLine("Пожалуйста, введите имя пользователя:");
				TryReadLine(out _username);
				Console.WriteLine("Пожалуйста, введите пароль:");
				TryReadLine(out _password);
			}
			Username = _username;
			Password = _password;
			Console.WriteLine("Авторизация прошла успешно");
		}
		/// <summary>
		/// Производит поиск совпадений пары "имя_пользователя,пароль" по файлу "login_data"
		/// </summary>
		/// <returns>true, если совпадение найдено, иначе - false</returns>
		static bool LoginSearchUser(string _username, string _password)
		{
			string[] _lines = File.ReadAllLines(LoginData);
			foreach (string _line in _lines)
			{
				string[] userData = _line.Split(',');

				if (userData.Length == 2)
				{
					if (userData[0] == _username && userData[1] == _password)
					{
						return true;
					}
				}
			}
			return false;
		}
		public static bool RegistrationStart()
		{
			if (File.Exists(LoginData))
			{
				using (StreamWriter writer = new StreamWriter(LoginData, true))
				{
					Registration();
					writer.WriteLine(_username + "," + _password);
					return true;
				}
			}
			else 
			{
				using (StreamWriter writer = new StreamWriter(LoginData))
				{
					Console.Write("Перед началом игры необходимо пройти регистрацию. ");
					Registration();
					writer.WriteLine(_username + "," + _password);
					return true;
				}
			}
		}
		static void Registration()
		{
			Console.WriteLine("Пожалуйста, придумайте имя пользователя. Использовать можно любые символы кроме ','  :");
			_username = Console.ReadLine();
			while (_username.Contains(","))
			{
				Console.WriteLine("Использование ','  недопустимо. Придумайте другое имя пользователя:");
				_username = Console.ReadLine();
			}

			Console.WriteLine("Пожалуйста, придумайте пароль. Использовать можно любые символы кроме ','  :");
			_password = Console.ReadLine();
			while (_password.Contains(","))
			{
				Console.WriteLine("Использование ','  недопустимо. Придумайте другой пароль:");
				_password = Console.ReadLine();
			}
			
			Console.WriteLine("Регистрация завершена.");
		}
		/// <summary>
		/// Метод TryReadLine считывает ввод пользователя с консоли, возвращая текст при нажатии Enter, прерывая ввод при нажатии F4 и завершает программу при нажатии Escape. Обрабатывает Backspace для удаления введённых символов. 
		/// </summary>
		/// <returns>true, если строка обработана, иначе - false</returns>
		public static bool TryReadLine(out string result)
		{
			var buf = new StringBuilder();
			for (; ; )
			{
				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.F4)
				{
					result = "";
					return false;
				}
				else if (key.Key == ConsoleKey.Enter)
				{
					result = buf.ToString();
					Console.WriteLine();
					return true;
				}
				else if (key.Key == ConsoleKey.Escape)
				{
					Console.WriteLine("Выход из программы...");
					Environment.Exit(0);
				}
				else if (key.Key == ConsoleKey.Backspace && buf.Length > 0)
				{
					buf.Remove(buf.Length - 1, 1);
					Console.Write("\b \b");
				}
				else if (key.KeyChar != 0)
				{
					buf.Append(key.KeyChar);
					Console.Write(key.KeyChar);
				}
			}
		}
	}
}
