namespace PropagationMw.Clients;

public class SampleClient(HttpClient httpClient) : ISampleClient
{
    private const string SampleUrl = "https://freetestapi.com/api/v1/actors";
    public Task<HttpResponseMessage> GetAsync()
    {
        return httpClient.GetAsync(SampleUrl);
    }
}