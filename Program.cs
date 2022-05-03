using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompilerMyLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            Console.WriteLine(calculator.Сalculate("min ((!4 / sin 6) ** 3) ? ((2**10) / 5)"));
        }
    }
}
