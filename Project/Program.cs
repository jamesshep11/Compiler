using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {
        static void Main(string[] args) {
            Parser P = new Parser("let var a : int ; var a : double in let var b : boolean in if a < 3 then b  := a < 3 else b := a = 1");
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
