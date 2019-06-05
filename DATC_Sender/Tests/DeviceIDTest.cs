using DATC_Sender.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DATC_Sender.Tests
{
    [TestFixture]
    class DeviceIDTest
    {
        [Test]
        public void GetDeviceID()
        {
            var id = DeviceID.ID;
            Assert.IsTrue(id.StartsWith(System.Environment.MachineName));
        }

        [Test]
        public void VerifyDeviceFileIsCreated()
        {
            if (File.Exists(DeviceID.FileName))
                File.Delete(DeviceID.FileName);

            var id = DeviceID.ID;
            Assert.IsTrue(File.Exists(DeviceID.FileName));
        }

        [Test]
        public void VerifyDeviceIDIsRepeatable()
        {
            if (File.Exists(DeviceID.FileName))
                File.Delete(DeviceID.FileName);

            var id1 = DeviceID.ID;
            
            var id2 = DeviceID.ID;
            Assert.AreEqual(id1, id2);
        }
    }
}
