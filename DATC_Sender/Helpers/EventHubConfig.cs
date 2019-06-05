using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DATC_Sender.Helpers
{
    class EventHubConfig
    {
        static EventHubConfiguration config = null;
        
        public static EventHubConfiguration Configuration
        {
            get
            {
                if (config == null)
                {
                    var json = File.ReadAllText("eventHub.json");
                    config = JsonConvert.DeserializeObject<EventHubConfiguration>(json);
                }

                return config;
            }
        }
    }

    class EventHubConfiguration
    {
        public string eventHubConnectionString { get; set; }
        public string eventHubName { get; set; }

        public string storageContainerName { get; set; }
        public string storageAccountName { get; set; }
        public string storageAccountKey { get; set; }
    }
}
