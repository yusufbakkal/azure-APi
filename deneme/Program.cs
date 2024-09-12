using Newtonsoft.Json;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();


app.MapGet("/traffic/{longitude}/{latitude}", async (double longitude, double latitude) =>
{
    
    string azureMapsKey = "57821jvsSOgwJSP3fM8ohN5jm50rRkSPGzh0qOfOe2Wc4mlsXaFEJQQJ99AHACYeBjFiG8lGAAAgAZMPS28l";  
    int zoom = 10;  
    string style = "relative";


    //string longitudeStr = longitude.ToString(new CultureInfo("tr-TR"));
    //string latiduteStr = latitude.ToString(new CultureInfo("tr-TR"));

    //string azureApiUrl = $"https://atlas.microsoft.com/traffic/flow/segment/json?subscription-key={azureMapsKey}&api-version=1.0&query={latitude},{longitude}&zoom={zoom}&style={style}";
    string azureApiUrl = $"https://atlas.microsoft.com/traffic/flow/segment/json?api-version=1.0&subscription-key={azureMapsKey}&style=relative&zoom=10&query={latitude.ToString().Replace(",", ".")},{longitude.ToString().Replace(",", ".")}";

    using (var client = new HttpClient())
    {
        var response = await client.GetAsync(azureApiUrl);
        var responseContent = await response.Content.ReadAsStringAsync(); 

        if (response.IsSuccessStatusCode)
        {
            
            var trafficData = JsonConvert.DeserializeObject<deneme.Models.AzureTrafficResponse>(responseContent);

            
            var trafficInfo = new TrafficInfo
            (
                longitude,
                latitude,
                trafficData?.flowSegmentData?.currentSpeed ?? 0,  
                trafficData?.flowSegmentData?.freeFlowSpeed ?? 0,  
                trafficData?.flowSegmentData?.currentTravelTime ?? 0, 
                trafficData?.flowSegmentData?.freeFlowTravelTime ?? 0 
            );

            return Results.Ok(trafficInfo);
        }
        else
        {
  
            Console.WriteLine($"Azure API hatasý: {responseContent}");
            return Results.Problem("Azure Maps API isteði baþarýsýz oldu. Detaylar: " + responseContent);
        }
    }
});

app.Run();

internal record TrafficInfo(double Longitude, double Latitude, double CurrentSpeed, double FreeFlowSpeed, int CurrentTravelTime, int FreeFlowTravelTime);


