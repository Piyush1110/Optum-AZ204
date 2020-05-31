using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class Program
{
    private const string _clientId = "ffee3152-84c0-4246-97ac-2970d031bbeb";
    private const string _tenantId = "fc73ed10-905f-4b66-85d6-ae10c14623d7";

    public static async Task Main(string[] args)
    {
        IPublicClientApplication app;

        app = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
            .WithRedirectUri("http://localhost")
            .Build();

        List<string> scopes = new List<string> 
        { 
            "user.read",
        };

        // 1: Authenticate user
        DeviceCodeProvider provider = new DeviceCodeProvider(app, scopes);

        GraphServiceClient client = new GraphServiceClient(provider);
        
        User myProfile = await client.Me
            .Request()
            .GetAsync();

        Console.WriteLine($"Name:\t{myProfile.DisplayName}");
        Console.WriteLine($"AAD Id:\t{myProfile.Id}");


        // 2: Get access token
        var acc = app.GetAccountsAsync().GetAwaiter().GetResult().First();        
        
        AuthenticationResult result = await app
            .AcquireTokenSilent(scopes, acc)
            .ExecuteAsync();

        string token = result.AccessToken;
        Console.WriteLine(token);

        ////3: Get user info from Microsoft Graph
        string endpoint = "https://graph.microsoft.com/v1.0/me";       
        var httpClient = new HttpClient();
        var authHeader = new AuthenticationHeaderValue("Bearer", token);

        httpClient.DefaultRequestHeaders.Authorization = authHeader;

        var response = httpClient.GetAsync(endpoint).GetAwaiter().GetResult();
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"\n\n");
            Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        else
        {
            Console.WriteLine("Error");
        }

    }
}