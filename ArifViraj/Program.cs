using System.Collections;

Console.WriteLine("Введите выражение, числовое выражение будет вычислено");

//считали строку с выражением
string str = Console.ReadLine();

int count = 0;
bool flag = false;
//проверка строки на корректность
for (int i = 0; i < str.Length; i++)
{
    if (flag || (str[i] >= 'A' && str[i] <= 'Z') ||
        (str[i] >= 'a' && str[i] <= 'z'))
    {
        flag = true;
    }
    if (
        (str[i] >= '0' && str[i] <= '9') ||
        (str[i] >= 'A' && str[i] <= 'Z') ||
        (str[i] >= 'a' && str[i] <= 'z') ||
        str[i] == '+' ||
        str[i] == '-' ||
        str[i] == '*' ||
        str[i] == '/' ||
        str[i] == '(' ||
        str[i] == ')'
       )
    {
        //подсчет открывающихся скобок
        if (str[i] == '(')
        {
            count++;
        }
        //подсчет закрывающихся скобок
        else if (str[i] == ')')
        {
            count--;
        }
    }
    else
    {
        Console.WriteLine("Введенное выражение содержит недопустимые символы");
        return;
    }
}

//в выражении число открывающихся и закрывающихся скобок не равно
if (count != 0)
{
    Console.WriteLine("Введенное выражение не содержит парные скобки");
    return;
}

//строка результата в полиз
string res = "";

//стек для операций
Stack temp = new Stack();

//цикл просмотра введенного выражения
for (int i = 0; i < str.Length; i++)
{
    //Если очередной элемент — число или имя переменной, то он сразу
    //переносится в результирующую строку
    if ((str[i] >= '0' && str[i] <= '9') ||
        (str[i] >= 'A' && str[i] <= 'Z') ||
        (str[i] >= 'a' && str[i] <= 'z'))
    {
        res += str[i];
        if (((i + 1) < str.Length) &&
            ((str[i + 1] >= '0' && str[i + 1] <= '9') ||
            (str[i + 1] >= 'A' && str[i + 1] <= 'Z') ||
            (str[i + 1] >= 'a' && str[i + 1] <= 'z')))
        {
            continue;
        }
        else
        {
            //добавление пробела между именами переменными или числами
            res += " ";
            continue;
        }
    }

    //Открывающаяся скобка всегда помещается в стек
    if (str[i] == '(')
    {
        temp.Push(str[i]);
        continue;
    }

    //Если встретилась закрывающаяся скобка
    if (str[i] == ')')
    {
        //из стека извлекаются все знаки операций до первой
        //открывающейся скобки и в порядке извлечения
        //переносятся в результирующую строку
        while (Convert.ToString(temp.Peek()) != "(")
        {
            res += temp.Pop();
            res += " ";
        }

        //открывающаяся скобка убираем
        temp.Pop();
        continue;
    }

    //Если встречается знак операции
    if (str[i] == '+' ||
        str[i] == '-' ||
        str[i] == '*' ||
        str[i] == '/')
    {
        //обработка унарного минуса
        //может стоять либо в самом начале выражения
        //либо после открывающейся скобки
        //в обоих случаях заносим в стек
        if (str[i] == '-' && (i == 0 || str[i - 1] == '('))
        {
            temp.Push("_");
        }
        //если это не унарный минус
        else
        {
            //если в вершине стека знак операции с более низким приоритетом
            //или открывающаяся скобка или ничего нет
            //то заносим в стек
            if (temp.Count == 0 ||
                Convert.ToString(temp.Peek()) == "(" ||
                ((Convert.ToString(temp.Peek()) == "+" ||
                Convert.ToString(temp.Peek()) == "-") &&
                (str[i] == '*' || str[i] == '/'))
                )
            {
                temp.Push(str[i]);
            }
            else
            {
                //знаки операций из стека, пока приоритет их
                //больше или равен приоритету данного знака, извлекаются из стека и
                //заносятся в результирующую строку

                //для * и /
                if (str[i] == '*' || str[i] == '/')
                {
                    if (temp.Count != 0)
                    {
                        while (Convert.ToString(temp.Peek()) != "(" &&
                           Convert.ToString(temp.Peek()) != "+" &&
                           Convert.ToString(temp.Peek()) != "-")
                        {
                            res += temp.Pop();
                            res += " ";
                            if (temp.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                    temp.Push(str[i]);
                }
                else
                {
                    if (temp.Count != 0)
                    {
                        while (Convert.ToString(temp.Peek()) != "(")
                        {
                            res += temp.Pop();
                            res += " ";
                            if (temp.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                    temp.Push(str[i]);
                }
            }
        }
    }
}

while (temp.Count != 0)
{
    res += temp.Pop();
    res += " ";
}

Console.WriteLine("Выражение в ПОЛИЗ: " + res);

//вычисление выражения если в нем не было переменных
if (!flag)
{
    string tmpStr = "";
    for (int i = 0; i < res.Length; i++)
    {
        if (res[i] == ' ')
        {
            continue;
        }

        if (res[i] >= '0' && res[i] <= '9')
        {
            tmpStr += res[i];
            if (res[i + 1] == ' ')
            {
                temp.Push(Convert.ToDouble(tmpStr));
                tmpStr = "";
            }
        }
        else
        {
            if (res[i]=='_')
            {
                temp.Push(Convert.ToDouble(-Convert.ToDouble(temp.Pop())));
            }
            else
            {
                double n2 = Convert.ToDouble(temp.Pop());
                double n1 = Convert.ToDouble(temp.Pop());
                switch (res[i])
                {
                    case '+':
                        temp.Push(n1 + n2);
                        break;
                    case '-':
                        temp.Push(n1 - n2);
                        break;
                    case '*':
                        temp.Push(n1 * n2);
                        break;
                    case '/':
                        temp.Push(n1 / n2);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    Console.WriteLine("Вычисление выражение: " + temp.Pop());
}