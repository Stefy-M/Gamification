using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WebSocketSharp;

namespace GamificationAccountGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No args entered. Need csv file pathname as argument");
                Console.ReadKey(true);
                return;
            }

            if(!File.Exists(args[0]))
            {
                Console.WriteLine("Incorrect pathname given.");
                Console.ReadKey(true);
                return;
            }

            using (var ws = new WebSocket("ws://69.166.48.217:60001/AccountCreation"))//("ws://dragonsnest.far/Laputa"))
            {

                ws.Connect();

                //each line in csv is line in array
                string[] lines = File.ReadAllLines(@args[0]);

                

                //remove commas, replace with spaces and add CREATE in front
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = lines[i].Replace(',', ' ');
                    lines[i] = lines[i].Insert(0, "CREATE ");
                    Console.WriteLine(lines[i] + " about to be sent to server");
                    ws.Send(lines[i]);
                }

                //done reading csv file
                Console.ReadKey(true);
                ws.Close();
            }

            

        }


    }
}
