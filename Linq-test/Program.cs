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
        public static int[] tab = { 1, 1, 1, 1, 1, 1, 10, 1, 1, 1, 1, 10, 2, 10, 10 };
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

            var res11 = LinqTasks.Task11();
            res11.ToList().ForEach(x => Console.WriteLine(x));

            Console.WriteLine("Task12");
            var res12 = LinqTasks.Task12();
            foreach (Emp emp in res12)
            {
                Console.WriteLine(emp);
            }


            var res13 = LinqTasks.Task13(tab);
            Console.WriteLine("Task 13: " + res13);

            Console.WriteLine("Task 14");
            var res14 = LinqTasks.Task14();
        }
    }
}
