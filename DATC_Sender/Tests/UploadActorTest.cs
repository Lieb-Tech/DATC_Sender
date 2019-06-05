using Akka.TestKit;
using Akka.TestKit.NUnit3;
using DATC_Sender.Actors;
using DATC_Sender.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATC_Sender.Tests
{
    [TestFixture]
    class UploadActorTest : TestKit
    {        
        [Test]
        public void BadConfigurationValues()
        {
            var eh = new EventHubConfiguration();
            var actor = ActorOf(UploadActor.Props(null, null));
            Watch(actor);
            ExpectTerminated(actor, TimeSpan.FromSeconds(5));            
        }

        [Test]
        public void BadRequest()
        {
            var eh = EventHubConfig.Configuration;
            ActorOf(UploadActor.Props(eh.eventHubConnectionString, "test.tet.test")).Tell(new UploadActor.UploadMessage(null), TestActor);
            ExpectMsg<UploadActor.NullRequest>(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void GoodRequestBadHub()
        {
            var eh = EventHubConfig.Configuration;
            ActorOf(UploadActor.Props(eh.eventHubConnectionString, "test.tet.test")).Tell(new UploadActor.UploadMessage(new object()), TestActor);
            ExpectMsg<UploadActor.UploadFailed>(TimeSpan.FromSeconds(10));
        }

        [Test]
        public void GoodRequestBadConfig()
        {
            var eh = EventHubConfig.Configuration;
            ActorOf(UploadActor.Props(eh.eventHubConnectionString + "test.tet.test", eh.eventHubName)).Tell(new UploadActor.UploadMessage(new object()), TestActor);
            ExpectMsg<UploadActor.UploadFailed>(TimeSpan.FromSeconds(10));
        }

        [Test]
        public void GoodRequest()
        {
            var eh = EventHubConfig.Configuration;
            ActorOf(UploadActor.Props(eh.eventHubConnectionString, "test")).Tell(new UploadActor.UploadMessage(new object()), TestActor);
            ExpectNoMsg(TimeSpan.FromSeconds(5));
        }
    }
}
