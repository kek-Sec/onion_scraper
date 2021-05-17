using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//class to scrape the indexing webpage (dark.fail) through its onion service to get list of active onions
namespace onion_scraper
{
    class WebRequest
    {
        /*
         * @param client -> The http client , HttpClient is intended to be instantiated once per application
         */
        public static async Task ScrapeAsync(HttpClient client,String url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine("{0} returned {1}",url,response.StatusCode);
                await FileWriter.WriteTextAsync("..\\..\\..\\scraped_sites\\dot_fail.html", responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
}
