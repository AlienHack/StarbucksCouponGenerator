using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace StarbucksCoupon
{
    internal class Program
    {
        private const string StarbucksCouponUrl = "https://starbucksthcampaign.com/c/quiz_2018_summer_3";
        private const string StarbucksOrigin = "https://starbucksthcampaign.com";

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";

        private const string ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        private const string ApplicationType = "application/json,  text/javascript, */*; q=0.01";

        private static void InitVoucher(string phoneNumber)
        {
            new Thread(() => { GetVoucher(phoneNumber); }).Start();
        }

        private static void GetVoucher(string phoneNumber)
        {
            var r = new Random(DateTime.Today.Millisecond);
            for (var i = 0; i < 1000; i++)
            {
                var cookieContainer = new CookieContainer();
                using (var webClientEx = new WebClientEx(cookieContainer))
                {
                    try
                    {
                        webClientEx.DownloadString(StarbucksCouponUrl);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    var payload =
                        $"mobilephone_1={phoneNumber}&mobilephone_2={phoneNumber}&mobilephone_3={phoneNumber}&mobilephone_4={phoneNumber}&question_91=326&question_92=330&question_97=350";

                    webClientEx.Headers.Add("Accept", ApplicationType);
                    webClientEx.Headers.Add("Content-Type", ContentType);
                    webClientEx.Headers.Add("Referer", StarbucksCouponUrl);
                    webClientEx.Headers.Add("Origin", StarbucksOrigin);
                    webClientEx.Headers.Add("User-Agent", UserAgent);

                    try
                    {
                        Console.Write("Request#" + (i + 1) + " Result...");
                        var result = JObject.Parse(webClientEx.UploadString(StarbucksCouponUrl, payload));
                        if (result["status"].ToString() == "1") Console.WriteLine("Success...");
                    }
                    catch (Exception)
                    {
                        // ignored
                        Console.WriteLine("Failed...");
                    }

                    Thread.Sleep(r.Next(100, 300));
                }
            }
        }

        //USAGE StarbucksCoupon <telephoneNumber> <threads>
        //Ex. StarbucksCoupon 0812345678 10
        private static void Main(string[] args)
        {
            for (var i = 0; i < int.Parse(args[1]); i++) InitVoucher(args[0]);
            Console.ReadLine();
        }

        public class WebClientEx : WebClient
        {
            public WebClientEx(CookieContainer container)
            {
                CookieContainer = container;
            }

            public WebClientEx()
            {
            }

            public CookieContainer CookieContainer { get; set; } = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                var r = base.GetWebRequest(address);
                if (r is HttpWebRequest request) request.CookieContainer = CookieContainer;
                return r;
            }

            protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
            {
                var response = base.GetWebResponse(request, result);
                ReadCookies(response);
                return response;
            }

            protected override WebResponse GetWebResponse(WebRequest request)
            {
                var response = base.GetWebResponse(request);
                ReadCookies(response);
                return response;
            }

            private void ReadCookies(WebResponse r)
            {
                if (r is HttpWebResponse response)
                {
                    var cookies = response.Cookies;
                    CookieContainer.Add(cookies);
                }
            }
        }
    }
}