using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Knapcode.TorSharp;
#if NETCOREAPP
using System.Runtime.InteropServices;
#endif

namespace onion_scraper
{
    class Program
    {
        private static void Main()
        {
            tor_sharp.init();
        }
    }

}