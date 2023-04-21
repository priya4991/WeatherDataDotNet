using System.Net.Http;
using System.Text.Json.Nodes;

class WeatherData {
    static readonly HttpClient client = new HttpClient();

    public static async Task Main(string [] args) {
        const string city = "London";
        const string apiKey = "9a5606ca8edd832b332a43246886ceb1";
        string url1 = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";
        Coords coords = await getCoords(url1);
        string url2 = $"https://api.openweathermap.org/data/2.5/weather?lat={coords.lat}&lon={coords.lon}&appid={apiKey}";


        await getCurrentWeather(url2, city);

 

    }

    public static async Task<Coords> getCoords(string url) {
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string res = await response.Content.ReadAsStringAsync();
            var obj = JsonArray.Parse(res);
            Coords coords = new Coords(
                obj[0]["lat"].GetValue<string>(),
                obj[0]["lon"].GetValue<string>()
                );
            return coords;
        }
        catch (Exception e)
        {
            return new Coords("","");
        }
    }

    public static async Task getCurrentWeather(string url, string city)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string res = await response.Content.ReadAsStringAsync();
            var obj = JsonObject.Parse(res);
            var weather = obj["weather"][0];
            Console.WriteLine($"City of {city} has {weather["description"]} today");
            Console.ReadLine();

            
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
    }



}

class Coords
{
    public string lat { get; }
    public string lon { get; }

    public Coords(string lat, string lon)
    {
        lat = lat;
        lon = lon;
    }
}





