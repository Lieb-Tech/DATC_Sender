using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using DATC_Sender.Actors;
using DATC_Sender.DataStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace DATC_Sender.Tests
{
    [TestFixture]
    class CollectorActorTest : TestKit
    {
        string goodIp = "192.168.1.148";
        string badIp = "192.168.1.1";

        TestProbe probe;
        public CollectorActorTest()
        {
            probe = CreateTestProbe();
        }

        [Test]
        public void MakeSureFailedDataIsReturned()
        {
            var actor = ActorOf(CollectorActor.Props(badIp, Helpers.DeviceID.ID));
            actor.Tell(new CollectorActor.CollectionRequest());
            ExpectMsg<CollectorActor.CollectionFailed>(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void MakeSureDataIsReturned()
        {
            ActorOf(CollectorActor.Props(goodIp, Helpers.DeviceID.ID)).Tell(new CollectorActor.CollectionRequest());
            var data = ExpectMsg<DeviceReading>(TimeSpan.FromSeconds(15));
            Assert.IsNotNull(data);
        }

        [Test]
        public void MakeSureDataIsRecent()
        {
            ActorOf(CollectorActor.Props(goodIp, Helpers.DeviceID.ID)).Tell(new CollectorActor.CollectionRequest());
            var data = ExpectMsg<DeviceReading>(TimeSpan.FromSeconds(10));
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            Assert.IsTrue(Math.Abs(data.now - now) < 5);
        }

        [Test]
        public void RequestDataAndValidateDevice()
        {
            ActorOf(CollectorActor.Props(goodIp, Helpers.DeviceID.ID)).Tell(new CollectorActor.CollectionRequest());
            var data = ExpectMsg<DeviceReading>(TimeSpan.FromMilliseconds(5000));
            Assert.AreEqual(data.deviceId, Helpers.DeviceID.ID);
        }

        [Test]
        public void RequestDataAndShouldBeAtleastOne()
        {
            ActorOf(CollectorActor.Props(goodIp, Helpers.DeviceID.ID)).Tell(new CollectorActor.CollectionRequest());
            var data = ExpectMsg<DeviceReading>(TimeSpan.FromMilliseconds(5000));
            Assert.IsTrue(data.aircraft.Any());
        }
    }
}
