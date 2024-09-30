using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherApp.Models;

internal class OpenWeatherAPIClient : IDisposable
{
	private readonly string apiKey;
    private readonly HttpClient httpClient;

	public OpenWeatherAPIClient()
	{
        apiKey = GetVariable();
		httpClient = new HttpClient();
	}

	private Uri GenerateRequestUrl(string queryString) => new Uri($"http://api.openweathermap.org/data/2.5/weather?appid={apiKey}&q={queryString}");
	private Uri GenerateRequestUrl(string longitude, string latitude) => new Uri($"http://api.openweathermap.org/data/2.5/forecast?appid={apiKey}&lon={longitude}&lat={latitude}");

    private string GetVariable()
    {
        Console.WriteLine("gety variable");
        return "2bd4b1e178c2896b75a5655c4f356c51";
     
    }
    
	private async Task<QueryResponse> QueryAsync(string queryString)
	{
		try
		{
			var jsonResponse = await httpClient.GetStringAsync(GenerateRequestUrl(queryString)).ConfigureAwait(false);
			var query = new QueryResponse(jsonResponse);
			return query.ValidRequest ? query : null;
		}
		catch (HttpRequestException)
		{
            MessageBox.Show("Check city name or make sure you are connected to internet!", "Invalid Request", MessageBoxButton.OK);
        }
        return null;
	}
	private async Task<ForecastQueryResponse> QueryAsync(string longitude, string latitude)
	{
        try
        {
            var jsonResponse = await httpClient.GetStringAsync(GenerateRequestUrl(longitude, latitude)).ConfigureAwait(false);
            var query = new ForecastQueryResponse(jsonResponse);
            return query.ValidRequest ? query : null;
        }
        catch (HttpRequestException)
        {
            MessageBox.Show("Check city name or make sure you are connected to internet!", "Invalid Request", MessageBoxButton.OK);
        }
        return null;
    }

    public QueryResponse Query(string queryString)
	{
		return Task.Run(async () => await QueryAsync(queryString).ConfigureAwait(false)).ConfigureAwait(false).GetAwaiter().GetResult();
	}
    public ForecastQueryResponse Query(string longitude, string latitude)
	{
		return Task.Run(async () => await QueryAsync(longitude, latitude).ConfigureAwait(false)).ConfigureAwait(false).GetAwaiter().GetResult();
	}

	private bool disposed;

	public void Dispose()
	{
  
        Dispose(true);
      
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
          
        }

        
        httpClient.Dispose();

        disposed = true;
    }
}
