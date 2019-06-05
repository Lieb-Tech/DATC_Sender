using Akka.Actor;
using DATC_Sender.DataStructures;
using DATC_Sender.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DATC_Sender.Actors
{
    class CollectorActor : ReceiveActor
    {
        public CollectorActor(string ipAddress, string deviceId)
        {
            string id = deviceId;
            WebClientExtended wce = new WebClientExtended();
            string url = $"http://{ipAddress}/dump1090-fa/data/aircraft.json?_=15562169425";

            Receive<CollectionRequest>(r =>
            {
                try
                {
                    // get data from device
                    var data = wce.DownloadString(url, 5);
                    // convert
                    var obj = JsonConvert.DeserializeObject<DeviceReading>(data);
                    // insert my id
                    obj.deviceId = id;                    
                    Sender.Tell(obj);
                }
                catch (Exception e)
                {
                    Sender.Tell(new CollectionFailed());
                };
            });
        }

        public static Props Props(string ipAddress, string deviceId) =>
            Akka.Actor.Props.Create(() => new CollectorActor(ipAddress, deviceId));

        #region
        internal class CollectionRequest
        {
            //  empty
        }

        internal class CollectionFailed
        {
            //  empty
        }
        #endregion
    }
}
