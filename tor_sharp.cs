using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Knapcode.TorSharp;
using System.Runtime.InteropServices;

namespace onion_scraper
{
    class tor_sharp
    {
        public static void init()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            // configure
            var settings = new TorSharpSettings
            {
                ZippedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorZipped"),
                ExtractedToolsDirectory = Path.Combine(Path.GetTempPath(), "TorExtracted"),
                PrivoxySettings =
                {
                    Port = 18118,
                },
                TorSettings =
                {
                    SocksPort = 19050,
                    AdditionalSockPorts = { 19052 },
                    ControlPort = 19051,
                    ControlPassword = "foobar",
                },
            };

            // output runtime information
            var message = new StringBuilder();
            message.Append($"Running on {settings.OSPlatform} OS and {settings.Architecture} architecture.");
            message.Append($" OS description: {RuntimeInformation.OSDescription}.");
            Console.WriteLine(message.ToString());
            Console.WriteLine();

            // download tools
            using (var httpClient = new HttpClient())
            {
                var fetcher = new TorSharpToolFetcher(settings, httpClient);
                var updates = await fetcher.CheckForUpdatesAsync();
                Console.WriteLine($"Current Privoxy: {updates.Privoxy.LocalVersion?.ToString() ?? "(none)"}");
                Console.WriteLine($" Latest Privoxy: {updates.Privoxy.LatestDownload.Version}");
                Console.WriteLine();
                Console.WriteLine($"Current Tor: {updates.Tor.LocalVersion?.ToString() ?? "(none)"}");
                Console.WriteLine($" Latest Tor: {updates.Tor.LatestDownload.Version}");
                Console.WriteLine();
                if (updates.HasUpdate)
                {
                    await fetcher.FetchAsync(updates);
                }
            }

            // execute
            using (var proxy = new TorSharpProxy(settings))
            {
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy(new Uri("http://localhost:" + settings.PrivoxySettings.Port))
                };

                using (handler)
                using (var httpClient = new HttpClient(handler))
                {
                    await proxy.ConfigureAndStartAsync();
                    await proxy.GetNewIdentityAsync();
                    Console.WriteLine("Onion IP -> {0}",await httpClient.GetStringAsync("http://api.ipify.org"));
                    Console.WriteLine(await httpClient.GetStringAsync("http://darkfailllnkf4vf.onion"));
                }
               // proxy.Stop();
            }
        }
    }
}
