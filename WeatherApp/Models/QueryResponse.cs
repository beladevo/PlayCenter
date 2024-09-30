using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace WeatherApp.Models;

internal class QueryResponse
{
    public bool ValidRequest { get; }
    public Coordinates Coordinates { get; }
    public List<Weather> WeatherList { get; } = new List<Weather>();
    public string Base { get; }
    public Main Main { get; }
    public double Visibility { get; }
    public Wind Wind { get; }
    public Rain Rain { get; }
    public Snow Snow { get; }
    public Clouds Clouds { get; }
    public Sys Sys { get; }
    public int DateTime { get; }
    public int Timezone { get; }
    public int ID { get; }
    public string Name { get; }
    public int Cod { get; }

    public QueryResponse(string jsonResponse)
    {
        var jsonData = JObject.Parse(jsonResponse);
        if (jsonData.SelectToken("cod").ToString() == "200")
        {
            ValidRequest = true;
            Coordinates = new Coordinates(jsonData.SelectToken("coord"));
            foreach (JToken weather in jsonData.SelectToken("weather"))
                WeatherList.Add(new Weather(weather));
            Base = jsonData.SelectToken("base").ToString();
            Main = new Main(jsonData.SelectToken("main"));
            Visibility = double.Parse(jsonData.SelectToken("visibility").ToString(), CultureInfo.InvariantCulture);
            Wind = new Wind(jsonData.SelectToken("wind"));
            Rain = new Rain(jsonData.SelectToken("rain"));
            Snow = new Snow(jsonData.SelectToken("snow"));
            Clouds = new Clouds(jsonData.SelectToken("clouds"));
            Sys = new Sys(jsonData.SelectToken("sys"));
            DateTime = int.Parse(jsonData.SelectToken("dt").ToString(), CultureInfo.InvariantCulture);
            Timezone = int.Parse(jsonData.SelectToken("timezone").ToString(), CultureInfo.InvariantCulture);
            ID = int.Parse(jsonData.SelectToken("id").ToString(), CultureInfo.InvariantCulture);
            Name = jsonData.SelectToken("name").ToString();
            Cod = int.Parse(jsonData.SelectToken("cod").ToString(), CultureInfo.InvariantCulture);
        }
        else ValidRequest = false;
    }
}
