using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Client
{
    class Program
    {
        enum ActionType
        {
            DOWNLOAD,
            COPY
        }

        static ActionType _action = ActionType.DOWNLOAD;
        static string _srcAccountName = string.Empty;
        static string _srcAccountKey = string.Empty;


        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("--- Azure Storage Backup utility program ---");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("\n\r\n");
            Console.WriteLine("Enter what action you would like to perform (download/copy):");

            bool validAction = false;

            do
            {
                var actionInput = Console.ReadLine();

                object parsedEnum;

                if (Enum.TryParse(typeof(ActionType), actionInput.ToUpper(), out parsedEnum))
                {
                    _action = (ActionType)parsedEnum;
                    validAction = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again (download/copy):");
                }
            } while (!validAction);



            Console.WriteLine("\n\r\nEnter source account name: ");
            _srcAccountName = Console.ReadLine();
            Console.WriteLine("\n\r\nEnter source account key: ");
            _srcAccountKey = Console.ReadLine();




            AzureTableStorage tableStorage =
                new AzureTableStorage()
                    .WithName(_srcAccountName)
                    .WithKey(_srcAccountKey)
                    .Initialize();



            foreach (var table in tableStorage.TableListGet())
            {
                Console.WriteLine($"Found table: {table.Name}");

                var entries = table.ExecuteQuery(new TableQuery()).ToList();

                foreach (var entry in entries)
                {
                    Console.WriteLine($"Found: {JsonConvert.SerializeObject(entry)}");
                }
            }
        }
    }
}
