using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MongoAnalysis
{
	public class Login
	{
		public ObjectId _id; // id doesn't matter
		public DateTime LoginTime;
		public DateTime LogoutTime;
	}

	public class Player
	{
		public ObjectId _id; // id doesn't matter
		public string Username;
		public string Password;
		public Login[] Logins;
		// Incremental, Seeker, Conqueror, Mastermind
		// are all objects serialized in JSON strings
		//
		// The game itself only (de)serializes these strings
		// which is why we can use Newtonsoft.Json to (de)serialize them
		// This database JSON requires MongoDB to do the (de)serializing
		// because of its weird extended format
		//
		// Assets/Incremental/Scripts/playerInfo.cs -> IncrementalData class
		// (This file also handles saving/loading the other strings)
		public string Incremental;
		// Assets/Seeker/Scripts/SaveLoad/GlobalControl.cs -> Seeker class
		public string Seeker;
		// Assets/Conqueror/Scripts/PlayerMovementScript.cs --> conqueror class
		// (Not a good place to put the code)
		public string Conqueror;
		// Assets/Mastermind/Scripts/StatisticsController.cs -> Statistics class
		// (This class is being serialized in XML which is then serialized into JSON)
		public string Mastermind;
		// Login Cookie (isn't that really insecure?)
		public string LoginCookie;
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
            var usernames = new List<string>();
            int loginCount = 0;

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
					p.Username, p.Password, p.Logins.Length);
                usernames.Add(p.Username);
                loginCount += p.Logins.Length;
				foreach (var d in p.Logins)
					Console.WriteLine("\t" + d.LoginTime + " ~ " + d.LogoutTime);
				Console.WriteLine();
			}

			// def'n of "played the game/active" being
			// ppl who have logged in at least once
			int numActive = 0;
			foreach (var p in players)
				if (p.Logins.Length > 0)
					++numActive;
			Console.WriteLine("Active Players/Accounts = {0}/{1}({2}%)",
				numActive, players.Count, 100 * numActive / players.Count);

            Console.WriteLine("Total Logins: {0}", loginCount);

            usernames.Sort();
            Console.WriteLine("Usernames:");
            foreach (string username in usernames)
            {
                Console.WriteLine("{0}", username);
            }

			// Rough code for finding # of active players during hours/days
			bool dateTimesOverlap(DateTime a1, DateTime a2, DateTime b1, DateTime b2)
			{
				return (b1 < a2 && b2 > a1);
			}

			bool loginTimesOverlap(Login l1, Login l2)
			{
				return dateTimesOverlap(l1.LoginTime, l1.LogoutTime,
					l2.LoginTime, l2.LogoutTime);
			}

			int numDaysToLog = 30;
			var loginTimeSegments = new List<Login>();
			var numActivePlayersSegments = new List<int>();
			for (int d = 0; d < numDaysToLog; ++d)
				for (int h = 0; h < 24; ++h)
					loginTimeSegments.Add(new Login
					{
						LoginTime = DateTime.Today.AddDays(1).Subtract(new TimeSpan(d, h + 1, 0, 0, 0)),
						LogoutTime = DateTime.Today.AddDays(1).Subtract(new TimeSpan(d, h, 0, 0, 1))
					});

			Console.WriteLine("\n\nNumber of people logged in during the hour:");
			for (int i = 0; i < loginTimeSegments.Count; ++i)
			{
				int numActivePlayers = 0;
				foreach (var p in players)
				{
					bool wasActive = false;
					foreach (var l in p.Logins)
					{
						wasActive = wasActive || loginTimesOverlap(l, loginTimeSegments[i]);
						if (wasActive) break;
					}
					if (wasActive) numActivePlayers++;
				}
				numActivePlayersSegments.Add(numActivePlayers);
			}

			Console.Write(" Hour Slot | ");
			for (int i = 0; i < 10; ++i)
				Console.Write(" " + i + " ");
			for (int i = 10; i < 24; ++i)
				Console.Write(i + " ");
			Console.Write("\n------------");
			for (int i = 0; i < 24; ++i)
				Console.Write("---");
			Console.WriteLine();

			for (int d = 0; d < numDaysToLog; ++d)
			{
				Console.Write(loginTimeSegments[d * 24].LoginTime.ToShortDateString() + " | ");
				for (int h = 23; h >= 0; --h)
				{
					if (loginTimeSegments[d * 24 + h].LoginTime > DateTime.Now)
						Console.Write(" " + '-' + " ");
					else
						Console.Write(" " + numActivePlayersSegments[d * 24 + h] + " ");
				}
				Console.WriteLine();
			}
		}
	}
}
