using System;
using System.Net.Http;
using ConsoleClient.DeliveryService;

namespace ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test1();

            Console.ReadLine();
        }

        private static async void Test1()
        {
            var client = new Client("https://localhost:44385/", new HttpClient());
            var all = await client.FillAllAsync();
        }
    }
}
