using Akka.Actor;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATC_Sender.Actors
{
    class UploadActor : ReceiveActor
    {
        private static EventHubClient eventHubClient;
        
        public UploadActor(string eventHubConnectionString, string eventHubName)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnectionString)
            {
                EntityPath = eventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            Receive<UploadMessage>(r =>
            {
                if (r.ToSend == null)
                    Sender.Tell(new NullRequest());
                else
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(r.ToSend);
                        eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(json))).Wait();
                    }
                    catch (Exception ex)
                    {
                        Sender.Tell(new UploadFailed(r.ToSend, ex));
                    }
                }
            });
        }

        public static Props Props(string eventHubConnectionString, string eventHubName) =>
            Akka.Actor.Props.Create(() => new UploadActor(eventHubConnectionString, eventHubName));

        
        internal class UploadFailed
        {
            public object ToSend { get; private set; }
            public Exception Exception { get; private set; }
            public UploadFailed(object toSend, Exception ex)
            {
                ToSend = toSend;
                Exception = ex;
            }
        }

        internal class NullRequest
        {
           
        }

        internal class UploadMessage
        {
            public object ToSend { get; private set; }
            public UploadMessage(object objectToSend)
            {
                ToSend = objectToSend;
            }
        }
    }
}
