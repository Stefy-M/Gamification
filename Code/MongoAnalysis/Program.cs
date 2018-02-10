using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MongoAnalysis
{
	[BsonIgnoreExtraElements] // so you don't have to serialize everything
	public class Player
	{
		// id doesn't matter
		public string Username;
		public string Password;
		// List of login logs
		// Logins are dictionaries
		// with keys of "LoginTime" and "LogoutTime"
		// and values of DateTime
		public List<Dictionary<string, DateTime>> Logins;
		// Incremental
		// Seeker (rpg)
		// Conquerer (shmup)
		// Mastermind (sudoku)
		// data.xml file details what's saved in the games
		// (or at least it should)
		// Login Cookie (isn't that really insecure?)
	}

	class Program
	{
		private static string playerDbPath = "playersV2.json";

		static void Main(string[] args)
		{
			Task newTask = Program.MainAsync(args);
			newTask.Wait();
		}

		static async Task MainAsync(string[] args)
		{
			string connectionString = "mongodb://ccain.eecs.wsu.edu:443/admin";
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase("test");
			bool isMongoLive
				= database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(5000);
			if (isMongoLive)
				Console.WriteLine("Connected!");
			else
			{
				Console.WriteLine("Could not connect!");
				return;
			}
			var collection = database.GetCollection<BsonDocument>("playersV2");

			// grabs newest version of player database
			// http://mongodb.github.io/mongo-csharp-driver/2.2/examples/exporting_json/
			using (var streamWriter = new StreamWriter(playerDbPath))
				using (var jsonWriter = new MongoDB.Bson.IO.JsonWriter(streamWriter))
					using (var cursor = await collection.FindAsync(new BsonDocument()))
					{
						var settings = new JsonWriterSettings
						{
							Indent = true, // Easier to read
						};
						while (await cursor.MoveNextAsync())
						{
							var batch = cursor.Current;
							foreach (var doc in batch)
								await streamWriter.WriteLineAsync(doc.ToJson(settings));
						}
					}

			// deserialize into player objects
			List<Player> players = new List<Player>();
			var sr = new StreamReader(playerDbPath);
			var reader = new MongoDB.Bson.IO.JsonReader(sr);
			while (!reader.IsAtEndOfFile())
				players.Add(BsonSerializer.Deserialize<Player>(reader));

			// leak every password
			foreach (var p in players)
			{
				Console.WriteLine("Username: {0}\nPassword: {1}\nLogins: {2}",
					p.Username, p.Password, p.Logins.Count);
				foreach (var d in p.Logins)
					Console.WriteLine(d["LoginTime"]
						+ " ~ " + d["LogoutTime"]);
				Console.WriteLine();
			}
			Console.ReadKey();

			// def'n of "played the game/active" being
			// ppl who have logged in at least once
			int numActive = 0;
			foreach (var p in players)
				if (p.Logins.Count > 0)
					++numActive;
			Console.WriteLine("Players/Accounts = {0}/{1}({2}%)",
				numActive, players.Count, 100 * numActive / players.Count);
		}
	}
}
