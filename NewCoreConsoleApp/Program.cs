using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using NewCoreClassLibrary;
using NewCoreConsoleApp.Data;

namespace NewCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NewCoreConsoleAppContext())
            {
                var data = context.People.ToList();
                foreach (var item in data)
                {
                    Console.WriteLine($"Name: {item.FirstName }");
                }
            }

            Console.ReadKey();

            return;

            Console.Write("Please write your name: ");
            var name = Console.ReadLine();

            Console.Write("Write your age: ");
            var yourAge = Console.ReadLine();

            var age = 0;

            int.TryParse(yourAge, out age);

            var utility = new Utility();
            var isAdult = utility.IsAdult(age);
            var adult = "not an adult ";
            if (isAdult)
            {
                adult = "an adult ";
            }

            Console.WriteLine($"\nHi {name}, you are {adult} person.");

            #region Commented Code


            //var isAdult = "not a adult ";
            //if (age >= 18)
            //{
            //    isAdult = "a adult ";
            //}

            //Console.WriteLine($"\nHi {name}, you are {isAdult} person.");

            #endregion Commented Code

            Console.ReadKey();
        }
    }
}
