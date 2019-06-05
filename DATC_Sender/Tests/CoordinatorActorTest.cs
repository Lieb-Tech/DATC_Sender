using Akka.TestKit.NUnit3;
using DATC_Sender.Actors;
using DATC_Sender.DataStructures;
using DATC_Sender.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATC_Sender.Tests
{
    [TestFixture]
    class CoordinatorActorTest : TestKit
    {
        [Test]
        public void EnsureThatChildIsSendingMessages()
        {
            var mock = ActorOf<MockUploadActor>("mock");
            var coord = ActorOf(CoordinatorActor.Props("192.168.1.148", mock));

            System.Threading.Thread.Sleep(20000);

            mock.Tell(new MockUploadActor.GetReadings(), TestActor);
            var r = ExpectMsg<List<DeviceReading>>(TimeSpan.FromSeconds(25));

            Assert.Greater(r.Count, 15);
        }
    }
}
