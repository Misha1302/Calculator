using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompilerMyLanguage
{
    class Calculator
    {
        public decimal Сalculate(string str) => Compute(Preparation(str));

        private int Priority(char x)
        {
            if (x == '(' || x == ')' || x == '↓' || x == '↑')
                return -1;
            if (x == '+' || x == '-')
                return 0;
            if (x == '*' || x == '/')
                return 1;
            return 2;
        }

        private bool IsDigit(char digit)
        {
            if (decimal.TryParse(digit.ToString(), out decimal num))
                return true;
            return false;
        }

        public List<string> Preparation(string equation)
        {
            List<string> res = new List<string>();
            List<char> stack = new List<char>();

            equation = PrepareString(equation);

            int x = 0;

            string digit = "";

            while (x < equation.Length)
            {
                digit = String.Empty;
                while (x < equation.Length)
                {
                    if (IsDigit(equation[x]) || equation[x] == ',')
                        digit += equation[x++];

                    else if (equation[x] == '-' && digit == string.Empty && IsDigit(equation[x + 1]))
                    {
                        digit += equation[x];
                        x += 1;
                    }
                    else
                    {
                        if (digit != "")
                        {
                            res.Add(digit);
                            digit = "";
                        }


                        if (equation[x] == ')')
                        {
                            while (stack[stack.Count - 1] != '(' && (stack.Count - 1) > 0)
                            {
                                res.Add(stack[stack.Count - 1].ToString());
                                stack.RemoveAt(stack.Count - 1);
                            }
                            stack.RemoveAt(stack.Count - 1);
                        }

                        else if (equation[x] == '(')
                            stack.Add(equation[x]);

                        else
                        {
                            if (equation[x] == '?') break;
                            int temp = Priority(equation[x]);
                            while (stack.Count != 0 && temp <= Priority(stack[stack.Count - 1]))
                            {
                                res.Add(stack[stack.Count - 1].ToString());
                                stack.RemoveAt(stack.Count - 1);
                            }
                            stack.Add(equation[x]);
                        }
                        break;
                    }
                }
                x += 1;
            }

            if (digit != "")
                res.Add(digit);

            for (int i = stack.Count - 1; i > -1; i--)
                res.Add(stack[i].ToString());

            return res;
        }

        public static string PrepareString(string equation)
        {
            var tempMassiv = equation.Split('"');
            for (int i = 0; i < tempMassiv.Length; i += 2)
            {
                tempMassiv[i] = tempMassiv[i]
                    .Replace(" ", String.Empty)
                    .Replace("**", "^")
                    .Replace("//", "⋌");

                tempMassiv[i] = tempMassiv[i].Replace(".", ",");
            }

            equation = String.Empty;
            for (int i = 0; i < tempMassiv.Length; i++)
            {
                if (tempMassiv.Length > 1)
                {
                    if (i % 2 == 0) tempMassiv[i] = tempMassiv[i] += "\"";
                    else tempMassiv[i] = tempMassiv[i] += "\" ";
                }
                equation += tempMassiv[i];
            }

            equation = Regex.Replace(equation, "(?!a-zA-Z)minus(?!a-zA-Z)", "╣", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)plus(?!a-zA-Z)", "∙", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)atg(?!a-zA-Z)", "≌", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)tan(?!a-zA-Z)", "≒", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)cos(?!a-zA-Z)", "Џ", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)sin(?!a-zA-Z)", "Ї", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)round(?!a-zA-Z)", "≈", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)min(?!a-zA-Z)", "↓", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)max(?!a-zA-Z)", "↑", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)root(?!a-zA-Z)", "√", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)% from(?!a-zA-Z)", "%", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
            equation = Regex.Replace(equation, "(?!a-zA-Z)-\\(", "⊕(", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

            return equation;
        }

        private decimal Compute(List<string> res)
        {
            int x = -1;
            string[] doubleOperands = new string[9] { "+", "-", "*", "/", "^", "↑", "↓", "⋌", "%" };
            string[] singlesOperands = new string[11] { "!", "|", "⊕", "Џ", "≈", "Ї", "√", "≌", "≒", "╣", "∙" };



            while (x < res.Count - 1)
            {
                x += 1;

                if (doubleOperands.Contains(res[x]))
                {
                    if (res[x] == "+") res[x - 2] = (Convert.ToDecimal(res[x - 2]) + Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "-") res[x - 2] = (Convert.ToDecimal(res[x - 2]) - Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "*") res[x - 2] = (Convert.ToDecimal(res[x - 2]) * Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "/") res[x - 2] = (Convert.ToDecimal(res[x - 2]) / Convert.ToDecimal(res[x - 1])).ToString();

                    else if (res[x] == "%") res[x - 2] = GetPercent(Convert.ToDecimal(res[x - 2]), Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "⋌") res[x - 2] = (Convert.ToDecimal(res[x - 2]) % Convert.ToDecimal(res[x - 1])).ToString();

                    else if (res[x] == "^") res[x - 2] = Math.Pow(Convert.ToDouble(res[x - 2]), Convert.ToDouble(res[x - 1])).ToString();

                    else if (res[x] == "↑") res[x - 2] = Math.Max(Convert.ToDecimal(res[x - 2]), Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "↓") res[x - 2] = Math.Min(Convert.ToDecimal(res[x - 2]), Convert.ToDecimal(res[x - 1])).ToString();

                    res.RemoveAt(x);
                    res.RemoveAt(x - 1);

                    x -= 2;
                }
                else if (singlesOperands.Contains(res[x]))
                {
                    if (res[x] == "!") res[x - 1] = Factorial(Convert.ToUInt64(Math.Round(Convert.ToDecimal(res[x - 1])))).ToString();
                    else if (res[x] == "√") res[x - 1] = GetRoot(Convert.ToDecimal(res[x - 1])).ToString();

                    else if (res[x] == "|") res[x - 1] = Math.Abs(Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "ђ") res[x - 1] = Math.Acos(Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "⊕") res[x - 1] = Math.Tan(Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "≌") res[x - 1] = Math.Atan(Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "Џ") res[x - 1] = Math.Cos(Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "≈") res[x - 1] = Math.Round(Convert.ToDecimal(res[x - 1])).ToString();
                    else if (res[x] == "Ї") res[x - 1] = Math.Sin(Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "≒") res[x - 1] = (-Convert.ToDouble(res[x - 1])).ToString();
                    else if (res[x] == "╣") res[x - 1] = (-Math.Abs(Convert.ToDouble(res[x - 1]))).ToString();
                    else if (res[x] == "∙") res[x - 1] = Math.Abs(Convert.ToDouble(res[x - 1])).ToString();


                    res.RemoveAt(x);
                    x -= 1;
                }
            }
            return Convert.ToDecimal(res[0]);
        }

        private ulong Factorial(ulong digit)
        {
            if (digit > 20)
                throw new Exception("Factorial must be less than 21\nResult is too big");

            ulong res = 1;
            for (ulong i = digit; i > 0; i--)
                res *= i;
            return res;
        }

        private object GetRoot(decimal digit, bool seeCount = false)
        {
            decimal middle = 0, left = 0, right = digit;

            int count = 0;
            while (right - left >= 0.000000000000000000000000009m)
            {
                count++;
                if (count > 1000) break;
                middle = (right + left) / 2;
                if (middle * middle > digit) right = middle;
                else left = middle;
            }

            middle = Math.Round(middle, 22);
            if (middle == Math.Truncate(middle))
            {
                if (seeCount)
                    return (Math.Round(middle), count);
                return Math.Round(middle);
            }

            if (seeCount)
                return (middle, count);
            return middle;
        }

        private decimal GetPercent(decimal digit, decimal percents) => digit * percents / 100;
    }
}
