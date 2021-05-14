using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//class to scrape the indexing webpage (dark.fail) through its onion service to get list of active onions
namespace onion_scraper
{
    class DotFail
    {
        /*
         * @param client -> The http client , HttpClient is intended to be instantiated once per application
         */
        public static async Task ScrapeAsync(HttpClient client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://darkfailllnkf4vf.onion");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
}
