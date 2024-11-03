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
		public static string Username { get { return _username; } }
		public static string Password { get { return _username; } }
		static string LoginData = "login_Data.txt";
		static string _username;
		static string _password;
		public static void LoginStart()
		{
			Console.WriteLine("Перед началом игры необходимо пройти авторизацию. Чтобы выйти из приложения, нажмите клавишу Esc.\nПожалуйста, введите имя пользователя:");
			TryReadLine(out _username);
			Console.WriteLine("Пожалуйста, введите пароль:");
			TryReadLine(out _password);
			LoginCheck(_username, _password);
		}

		public static void LoginCheck(string _username, string _password)
		{
			while (LoginSearchUser(_username, _password) == false)
			{
				Console.WriteLine("Имя пользователя или пароль введены неверно. Попробуйте еще раз. Чтобы выйти из приложений, нажмите клавишу Esc");
				Console.WriteLine("Пожалуйста, введите имя пользователя:");
				TryReadLine(out _username);
				Console.WriteLine("Пожалуйста, введите пароль:");
				TryReadLine(out _password);
			}
			Console.WriteLine("Авторизация прошла успешно");
		}

		public static bool LoginSearchUser(string _username, string _password)
		{
			if (File.Exists(LoginData))
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
			else
			{
				// Создаем файл и записываем в него текст
				using (StreamWriter writer = new StreamWriter(LoginData))
				{
					Console.WriteLine("Пожалуйста, придумайте имя пользователя. Использовать можно любые символы кроме ",":");
					_username = Console.ReadLine();
					while (_username.Contains(","))
					{
						Console.WriteLine("Использование "," недопустимо:");
						_username = Console.ReadLine();
					}					
					Console.WriteLine("Пожалуйста, придумайте пароль:");
					_password = Console.ReadLine();
					while (_password.Contains(","))
					{
						Console.WriteLine("Использование ", " недопустимо:");
						_password = Console.ReadLine();
					}
					writer.WriteLine(_username + "," + _password);
					Console.WriteLine("Регистрация завершена.");
				}
				return false;
			}
		}
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
