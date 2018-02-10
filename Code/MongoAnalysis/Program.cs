using System;
using MongoDB.Bson
using MongoDB.Driver


namespace MongoAnalysis
{

    class Program
    {
		private string connectionString = "mongodb://auser:g0HUsk!E$@ccain.eecs.wsu.edu:441";
        static void Main(string[] args)
        {
            var client = new MongoClient(connectionString);
			var database = client.getDatabase("PlayersV2");

        }
    }
}
