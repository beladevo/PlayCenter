using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace WeatherApp.Models;

internal class Forecast
{
    public int DateTime { get; }
    public Main Main { get; }
    public List<Weather> WeatherList { get; } = new List<Weather>();

    public Forecast(JToken forecastData)
    {
        if (forecastData != null)
        {
            DateTime = int.Parse(forecastData.SelectToken("dt").ToString(), CultureInfo.InvariantCulture);
            Main = new Main(forecastData.SelectToken("main"));
            foreach(JToken weather in forecastData.SelectToken("weather"))
                WeatherList.Add(new Weather(weather));
        }
    }


}

internal class Day
{
    public TemperatureUnit Temperature { get; }
    public string Name { get; }
    public string Description { get; }
    public int ID { get; }
    public string Icon { get; }

    public Day(TemperatureUnit temperature, string name, string description,int id, string icon)
    {
        Temperature = temperature;
        Name = name;
        Description = description;
        ID = id;
        Icon = icon;
    }
}