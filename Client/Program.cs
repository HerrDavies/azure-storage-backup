using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

                var tablecontents = tableStorage.TableContentsGet(table.Name).ToList();


                tableStorage.TableContentsSave($"{table.Name}_new", tablecontents);






                Console.WriteLine($"Found table: {table.Name}");

                var entries = table.ExecuteQuery(new TableQuery()).ToList();

                var entriesToSave = new List<IDictionary<string, object>>();

                foreach (var entry in entries)
                {

                    var newEntry = new ExpandoObject() as IDictionary<string, object>;

                    newEntry.Add("PartitionKey", entry.PartitionKey);
                    newEntry.Add("RowKey", entry.RowKey);
                    newEntry.Add("Timestamp", entry.Timestamp);

                    foreach (var property in entry.Properties)
                    {
                        newEntry.Add(property.Key, property.Value);
                    }

                    entriesToSave.Add(newEntry);

                    Console.WriteLine($"Found: {JsonConvert.SerializeObject(entry)}");
                }
            }






        }
    }
}
