using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DATC_Sender.Helpers
{
    class WebClientExtended
    {
        public string DownloadString(string url, int timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout * 1000;
            request.ReadWriteTimeout = timeout * 1000;
            var resp = (HttpWebResponse)request.GetResponse();
            string data = "";
            using (Stream stream = resp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                data = reader.ReadToEnd();
            }

            return data;
        }
    }
}
