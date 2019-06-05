using DATC_Sender.Actors;
using System;

namespace DATC_Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up system");

            using (var sys = Akka.Actor.ActorSystem.Create("DATC"))
            {
                var eh = Helpers.EventHubConfig.Configuration;
                //var uploader = sys.ActorOf(Actors.UploadActor.Props(eh.eventHubConnectionString, eh.eventHubName));
                var uploader = sys.ActorOf(MockUploadActor.Props(), "mockMcMockFace");
                var coord = sys.ActorOf(CoordinatorActor.Props("192.168.1.148", uploader));

                Console.ReadLine();
                Console.ReadLine();
            }
        }
    }
}
