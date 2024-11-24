using System;
using System.Net;
using System.Web;

namespace IMS.Services
{
    public static class SMSService
    {

       

        //Console.Read(); //to keep console window open if trying in visual studio
        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            string MyApiKey = "923122533963-3dfe6bf4-5079-41e1-8305-a5ef4a1e1e87"; //Your API Key At Sendpk.com
           
            String URI = "https://sendpk.com" +
            "/api/sms.php?" +
            "api_key=" + MyApiKey +
            "&sender=" + Masking +
            "&mobile=" + toNumber +
            "&message=" + Uri.UnescapeDataString(MessageText); // Visual Studio 10-15
                                                               //"//&message=" + System.Net.WebUtility.UrlEncode(MessageText);// Visual Studio 12
            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return "404:URL not found :" + URI;
                            break;
                        case HttpStatusCode.BadRequest:
                            return "400:Bad Request";
                            break;
                        default:
                            return httpWebResponse.StatusCode.ToString();
                    }
                }
            }
            return null;
        }
    }
}
    

