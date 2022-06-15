using LinqTutorials;
using LinqTutorials.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_test
{
    internal class Program
    {
        static public void Main(String[] args)
        {
            var res = LinqTasks.Task5();
            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Task6:");
            var res6 = LinqTasks.Task6();

            var res7 = LinqTasks.Task7();
            Console.WriteLine("Task7: ");
            foreach (var e in res7)
            {
                Console.WriteLine(e);
            }

            var res8 = LinqTasks.Task8();
            Console.WriteLine("Test8: " + res8);

            var res9 = LinqTasks.Task9();
            Console.WriteLine("Test9: " + res9);
        }
    }
}
