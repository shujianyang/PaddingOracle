using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PaddingOracle
{
    class Pad
    {
        public Pad(string ct)
        {
            CipherText = ct;
        }

        public int getResponseCode()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TARGET + CipherText);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return (int)response.StatusCode;
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                int code = (int)response.StatusCode;
                string m= "";
                m = m + "Message: " + we.Message + "\n";
                //m = m + "Code: " + code + "\n";
                //m = m + "Code: " + (int)response.StatusCode + "\n";
                //m = m + "Status: " + we.Status ;
                //Console.WriteLine(m);

                return code;
            }
        }

        private const string TARGET = @"http://crypto-class.appspot.com/po?er=";
        public string CipherText { get; set; }
    }
}
