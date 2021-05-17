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
        public static String[] onion_labels = {
            "DarkNet Live",
            "Recon",
            "The Majestic Garden",
            "Kilos",
            "White House Market",
            "The Versus Project",
            "Asap Market",
            "Vice City Market",
            "Canna Home",
            "CannaZone",
            "Dark0de Market",
            "Televend",
            "ToRReZ Market",
            "Tor Market",
            "Hydra",
            "Canada HQ",
            "CryptoStamps",
            "BBC",
            "Deutsche Welle",
            "The NewYork Times",
            "End Chan",
            "National Police of the Netherlands",
            "CIA",
            "USA's NCIDE Task Force"
        };
        public static String[] onions = {
            "http://darkzzx4avcsuofgfez5zq75cqc4mprjvfqywo45dfcaxrwqg6qrlfid.onion",
            "http://recon222tttn4ob7ujdhbn3s4gjre7netvzybuvbq2bcqwltkiqinhad.onion",
            "http://2oywvwmtzdelmiei.onion",
            "http://dnmugu4755642434.onion",
            "http://7yipwxdv5cfdjfpjztiz7sv2jlzzjuepmxy4mtlvuaojejwhg3zhliqd.onion",
            "http://pqqmr3p3tppwqvvapi6fa7jowrehgd36ct6lzr26qqormaqvh6gt4jyd.onion",
            "http://asap4u7rq4tyakf5gdahmj2c77blwc4noxnsppp5lzlhk7x34x2e22yd.onion",
            "http://vice2e3gr3pmaikukidllstulxvkb7a247gkguihzvyk3gqwdpolqead.onion",
            "http://cannabmuc64fbglolpkvnmqynsx226pg27rgimfe3gye3emgtgodohqd.onion",
            "http://cannazonceujdye3.onion",
            "http://darkoddrkj3gqz7ke7nyjfkh7o72hlvr44uz5zl2xrapna4tribuorqd.onion",
            "http://televenkzhxxxe6sw4fntkm4csj6s4csqkuczqhrz6aw7ae3me2tjlyd.onion",
             "http://333f7gpuishjximodvynnoisxujicgwaetzywgkxoxuje5ph3qyqjuid.onion",
             "http://rrlm2f22lpqgfhyydqkxxzv6snwo5qvc2krjt2q557l7z4te7fsvhbid.onion",
             "http://hydraruzxpnew4af.onion",
             "http://canadahq2lo3logs.onion",
             "http://lgh3eosuqrrtvwx3s4nurujcqrm53ba5vqsbim5k5ntdpo33qkl7buyd.onion",
             "http://bbcnewsv2vjtpsuy.onion",
             "http://dwnewsvdyyiamwnp.onion",
             "https://nytimes3xbfgragh.onion",
             "http://endchan5doxvprs5.onion",
             "http://politiepcvh42eav.onion",
             "http://ciadotgov4sjwlzihbbgxnqg3xiyrg7so2r2o3lt5wz5ypk4sxyjstad.onion",
             "http://ncidetfs7banpz2d7vpndev5somwoki5vwdpfty2k7javniujekit6ad.onion"
        };

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
                    Console.WriteLine("Onion IP -> {0}", await httpClient.GetStringAsync("http://api.ipify.org"));

                    for (int i = 0; i < onions.Length; i++)
                    {
                        await WebRequest.ScrapeAsync(httpClient, i);
                    }

                    // proxy.Stop();
                }
            }
        }
    }
}
