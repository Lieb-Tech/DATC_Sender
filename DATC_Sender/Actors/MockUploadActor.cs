
using Akka.Actor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DATC_Sender.Actors
{
    public partial class MockUploadActor : ReceiveActor
    {
        public List<DataStructures.DeviceReading> readings = new List<DataStructures.DeviceReading>();

        public MockUploadActor()
        {
            Receive<UploadActor.UploadMessage>(r =>
            {
                readings.Add(r.ToSend as DataStructures.DeviceReading);
                if (!Directory.Exists("TestData"))
                    Directory.CreateDirectory("TestData");

                File.WriteAllText("TestData/DeviceReading_" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".json", JsonConvert.SerializeObject(r));
            });
            Receive<GetReadings>(r =>
            {
                Sender.Tell(readings);
            });
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new MockUploadActor());

        internal class GetReadings
        {
        }
    }
}
