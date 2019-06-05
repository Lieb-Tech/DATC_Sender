using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DATC_Sender.Tests
{
    [TestFixture]
    class EventHubConfigtest
    {
        [Test]
        public void GetGoodConfig()
        {
            var data = Helpers.EventHubConfig.Configuration;
            Assert.IsNotNull(data);
        }

        [Test]
        public void GetGoodConfigValue()
        {
            var data = Helpers.EventHubConfig.Configuration;
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.eventHubConnectionString);
            Assert.IsNotNull(data.eventHubName);
        }

        [Test]
        public void TryMissingConfig()
        {
            if (File.Exists("eventHub.json"))
                File.Delete("eventHub.json");

            Assert.Catch(() => { var c = Helpers.EventHubConfig.Configuration;  });
        }
    }
}
