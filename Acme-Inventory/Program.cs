using System;

namespace Acme_Inventory
{
    class Program
    {
        static void Main(string[] args)
        {
            var restart = "";
            do
            {
                //main part of program
                Console.Write("Do you wish to calculate another? (YES/NO) ");
                restart = Console.ReadLine().ToUpper();
                while ((restart != "YES") && (restart != "NO")) //????
                {
                    Console.WriteLine("Error");
                    Console.WriteLine("Do you wish to calculate another? (YES/NO) ");
                    restart = Console.ReadLine().ToUpper();
                }
            } while (restart == "YES");
        }
    }
}