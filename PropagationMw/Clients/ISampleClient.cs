namespace PropagationMw.Clients;

public interface ISampleClient
{
    Task<HttpResponseMessage> GetAsync();
}