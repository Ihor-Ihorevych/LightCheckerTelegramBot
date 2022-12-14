using RestSharp;

using TGBot.Data.DTO;

namespace TGBot.Business_Layer.Core.Services;

public interface ILightCheckService : IDisposable
{
    /// <summary>
    ///     Gets the current light status by calling the API
    /// </summary>
    /// <param name="code">
    ///     Some code
    /// </param>
    /// <returns>
    ///     The <see cref="Report" /> object.
    /// </returns>
    Task<Report?> GetReportAsync(string code);
}

public class LightCheckService : ILightCheckService
{
    private readonly RestClient _restClient;
    private bool _disposed;

    public LightCheckService(RestClient restClient) => _restClient = restClient;


    /// <inheritdoc />
    public async Task<Report?> GetReportAsync(string code)
    {
        RestRequest request = new("/api/GetPowerStatus");
        request.AddQueryParameter("code", code);
        RestResponse<Report?> response = await _restClient.ExecuteAsync<Report?>(request);
        return response.IsSuccessful ? response.Data : null!;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    virtual protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            return;
        }

        if (disposing)
        {
            _restClient.Dispose();
        }

        _disposed = true;
    }
}


