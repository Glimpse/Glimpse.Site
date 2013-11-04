using Glimpse.Issues;

public class HttpClientFactory
{
    public IHttpClient CreateHttpClient(string githubKey, string githubSecret)
    {
        const string baseAddress = "https://api.github.com/";
        const string mediaType = "application/json";
        if(githubKey == null || githubSecret == null)
            return new BasicHttpClient(baseAddress, mediaType);
        return new AuthenticatedHttpClient(baseAddress,mediaType,githubKey,githubSecret);
    }
}