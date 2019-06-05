using Akka.Actor;
using DATC_Sender.DataStructures;
using DATC_Sender.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATC_Sender.Actors
{
    class CoordinatorActor : ReceiveActor
    {
        IActorRef collector;
        IActorRef uploader;
        ICancelable sched;
        double lastReadingEpoch = 0;

        protected override void PostStop()
        {
            base.PostStop();
            sched.Cancel();
            Console.WriteLine("Stopping Coordinator");
        }            

        public CoordinatorActor(string ipAddress, IActorRef uploadActor)
        {
            DateTimeOffset lastGoodMessage = DateTimeOffset.Now;
            Console.WriteLine("Starting Coordinator");

            uploader = uploadActor;
            collector = Context.ActorOf(CollectorActor.Props(ipAddress, Helpers.DeviceID.ID));
            sched = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromMilliseconds(1),
                TimeSpan.FromMilliseconds(200),
                collector,
                new CollectorActor.CollectionRequest(),
                Self);

            Receive<CollectorActor.CollectionFailed>(r =>
            {
                /// TODO: Notify lack of data                
            });

            Receive<DeviceReading>(r =>
            {
                // only update if newer
                if (lastReadingEpoch != r.now)
                {
                    Console.WriteLine($"{r.aircraft.Count}");
                    uploader.Tell(new UploadActor.UploadMessage(r));
                }                

                lastGoodMessage = DateTimeOffset.UtcNow;
                lastReadingEpoch = r.now;                                
            });
        }

        public static Props Props(string ipAddress, IActorRef uploader) =>
            Akka.Actor.Props.Create(() => new CoordinatorActor(ipAddress, uploader));

        internal class NoRecentMessages
        {

        }
    }
}
