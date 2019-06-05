using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace DATC_Sender.Helpers
{
    class DeviceID
    {
        public const string FileName = "DeviceID";

        // a persistant pseudo unique device id
        public static string ID
        {            
            get
            {
                string info = "";
                if (File.Exists(FileName))
                {
                    info = File.ReadAllText(FileName);
                }
                else
                {                    
                    info = $"{System.Environment.MachineName}:{Guid.NewGuid().ToString()}";
                    File.WriteAllText(FileName, info);
                }
                
                return info;
            }
        }
    }
}
