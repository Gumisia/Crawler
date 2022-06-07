using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Crawler
{
    public class Program
    {

        public static async Task Main(string[] args)
        {

            if (args[0] == null) throw new ArgumentNullException("Main");

            {
                Uri uri;
                bool result = Uri.TryCreate(args[0], UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

                if (!result) throw new ArgumentException("Argument is not Not URL");

                string websiteUrl = args[0];

                Regex regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z](?:[a-z]*[a-z])?", RegexOptions.Compiled);

                HttpClient httpClient = new HttpClient();
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);

                    string stringResponse;

                    if (response.IsSuccessStatusCode)
                    {

                        stringResponse = await response.Content.ReadAsStringAsync();

                        MatchCollection matches = regex.Matches(stringResponse);

                        if (matches.Count == 0) { Console.WriteLine("Nie znaleziono adresów email"); }
                        else
                        {
                            Console.WriteLine("{0} matches found.", matches.Count);

                            HashSet<string> hashSet = new HashSet<string>();

                            foreach (Match match in matches)
                            {
                                hashSet.Add(match.Value);
                            }

                            DisplaySet(hashSet);

                        }

                    }


                    httpClient.Dispose();
                    response.Dispose();

                }
                catch (HttpRequestException ex) { Console.WriteLine("B³¹d w czasie pobierania strony"); }
            }


        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private static void DisplaySet(HashSet<string> collection)
        {

            foreach (string i in collection)
            {
                Console.WriteLine("{0}", i);
            }
        }
    }
}


