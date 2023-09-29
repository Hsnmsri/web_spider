using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        String DomainFullURL;
        String DomainBaseURL;
        int DomainPORT;
        WelcomePrint();
        DomainBaseURL = getDomain();
        DomainPORT = getPort();
        DomainFullURL = (DomainPORT == 443) ? "https://" : "http://";
        DomainFullURL += DomainBaseURL;
        PrintFullURL(DomainFullURL + ":" + DomainPORT);
        PingDomain(DomainBaseURL);
        Console.WriteLine();
        ExtractLinks(DomainFullURL).Wait();
        Console.ReadKey();
    }
    static void WelcomePrint()
    {
        ConsoleColor defaultConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("WebSpider");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Developed by Mr.Mansouri");
        Console.WriteLine("v1.0.0");
        Console.ForegroundColor = defaultConsoleColor;
        Console.WriteLine("-");
    }
    static String getDomain()
    {
        ConsoleColor defaultConsoleColor = Console.ForegroundColor;
        Console.Write("Domain");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("(without http:// or https://)");
        Console.ForegroundColor = defaultConsoleColor;
        Console.Write(":");
        while (true)
        {
            String? temp = Console.ReadLine();
            if (temp != "" && temp != null) return temp;
        }
    }
    static int getPort()
    {
        ConsoleColor defaultConsoleColor = Console.ForegroundColor;
        Console.Write("Port");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("(default 80,443)");
        Console.ForegroundColor = defaultConsoleColor;
        Console.Write(":");
        switch (Console.ReadLine())
        {
            case "80":
                {
                    return 80;
                }
            case "443":
                {
                    return 443;
                }
            default:
                {
                    return 80;
                }
        }
    }
    static void PrintFullURL(String FullURL)
    {
        ConsoleColor defaultConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("--");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write("Domain : ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(FullURL);
        Console.WriteLine("\n--");
        Console.ForegroundColor = defaultConsoleColor;
    }
    static async Task ExtractLinks(String URL)
    {
        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Specify the URL of the API you want to call
                string apiUrl = URL;

                // Send a GET request to the API
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the response is successful (status code 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Read the content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    string inputText = content;

                    // Define a regular expression pattern to match both HTTP and HTTPS links
                    string pattern = @"https?://\S+";

                    // Create a Regex object
                    Regex regex = new Regex(pattern);

                    // Find all matches in the input text
                    MatchCollection matches = regex.Matches(inputText);

                    // Extract and print the links
                    foreach (Match match in matches)
                    {
                        string link = match.Value;
                        PrintOutput("link", link);
                    }
                }
                else
                {
                    // Handle the error (e.g., log or display an error message)
                    Console.WriteLine($"API call failed with status code {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exceptions (e.g., network issues)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
    static void PingDomain(String URL)
    {
        string targetDomain = URL; // Replace with the domain you want to ping

        Ping pingSender = new Ping();
        PingReply reply = pingSender.Send(targetDomain);

        if (reply.Status == IPStatus.Success)
        {
            PrintOutput("IP Address", reply.Address.ToString());
            PrintOutput("Ping", $"{reply.RoundtripTime} ms");
        }
        else
        {
            Console.WriteLine($"Ping to {targetDomain} failed.");
            Console.WriteLine($"Status: {reply.Status}");
        }
    }
    static void PrintOutput(String Title, String Content)
    {
        ConsoleColor defaultConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write(Title + ": ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(Content);
        Console.ForegroundColor = defaultConsoleColor;
        Console.WriteLine("");
    }
}
