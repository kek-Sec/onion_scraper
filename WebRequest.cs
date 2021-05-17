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
         * @param index -> index used for arrays containing labels and urls
         */
        public static async Task ScrapeAsync(HttpClient client,int index)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(tor_sharp.onions[index]);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine("{0} returned {1}",tor_sharp.onion_labels[index],response.StatusCode);
                await FileWriter.WriteTextAsync("..\\..\\..\\scraped_sites\\" + tor_sharp.onion_labels[index] + ".html", responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException thrown by -> {0}!",tor_sharp.onion_labels[index]);
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

    }
}
