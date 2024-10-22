# Лабораторная работа №2

По дисциплине "Тестирование и отладка программного обеспечения"
БО231ПИН Максимова К.В. 
2024


Данный проект содержит код консольного приложения, в котором загадывается 4-х значный код (цифры в одном коде не повторяются). Цель пользователя - отгадать его. Количество попыток, потребовавшееся пользователю для победы записывается в "game_log.txt". Для запуска процесса игры пользователю необходимо пройти авторизацию. Имена пользователей и пароли хранятся в "login_data.txt": данные разных пользователей разделены пустой строкой, имя пользователя находится на i-строке, пароль - на i+1.
## Авторизация
Авторизация устроена следующим образом: 
1. Пользователь вводит имя пользователя и пароль.
2. Эти данные передаются в метод StartLogin, в котором организован цикл while. Он работает до тех пор, пока метод Login не вернет значение true.
3. Метод Login проходит через все строки файла "login_data.txt" с шагом 3. Если i и i+1 строки соответственно равны имени пользователя и паролю, то авторизация считается успешной.
4. <im src="https://github.com/user-attachments/assets/e1dd6042-871e-427a-9ebc-635740969cd1" width="500">
![image](https://github.com/user-attachments/assets/e1dd6042-871e-427a-9ebc-635740969cd1)

## Генерация чисел
За генерацию числа отвечает метод Rnd. 
```c#
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
```
