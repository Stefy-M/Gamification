using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;


namespace MongoAnalysis
{

    class Program
    {
        static void Main(string[] args)
        {
            Task newTask =  Program.MainAsync(args);
            newTask.Wait();

        }
        static async Task MainAsync(string[] args)
        {
            string connectionString = "mongodb://ccain.eecs.wsu.edu:443/admin";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            bool isMongoLive = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            if(isMongoLive)
            {
                Console.WriteLine("Connected!");
            }
            else
            {
                Console.WriteLine("Could not connect");

            }
            var collection = database.GetCollection<BsonDocument>("playersV2");

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine(document);
                        Console.WriteLine();
                    }
                }
            }
        }

}



