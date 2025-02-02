using Contact.Persistence.Domain.ResultServices;
using Contact.Persistence.Domain.Services;
using Serilog;
using System.Net.Http.Headers;

namespace Contact.Persistence.Infrastructure.ServicesAccess;
public class ContactQueryServiceApi(
    IHttpClientFactory httpClientFactory,
    ILogger logger) : Base, IContactQueryServiceApi
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger _logger = logger;

    public async Task<Result<ThereIsContactResult>> ThereIsContactAsync(int ddd, string phoneNumber, string token)
    {
        _logger.Information($"{nameof(ThereIsContactAsync)} - Initiating call to Contact.Query Api.");

        var output = new Result<ThereIsContactResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("ContactQueryApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = string.Format("/api/v1/contact/there-is-contact/{0}/{1}", ddd, phoneNumber);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseApi = DeserializeResponseObject<Result<ThereIsContactResult>>(content);

                _logger.Information($"{nameof(ThereIsContactAsync)} - Ended call to Contact.Query Api.");

                return responseApi;
            }

            var failMessage = $"{nameof(ThereIsContactAsync)} - An error occurred when calling the Contact.Query Api. StatusCode: {response.StatusCode}";

            _logger.Error(failMessage);

            return output.Failure(failMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"{nameof(ThereIsContactAsync)} - An error occurred when calling the Contact.Query Api. Error: {ex.Message}";

            _logger.Error(errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ContactResult>> RecoverContactByIdAsync(Guid id, string token)
    {
        _logger.Information($"{nameof(ThereIsContactAsync)} - Initiating call to Contact.Query Api.");

        var output = new Result<ContactResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("ContactQueryApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = string.Format("/api/v1/contact/recover-by-id/{0}", id);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseApi = DeserializeResponseObject<Result<ContactResult>>(content);

                _logger.Information($"{nameof(ThereIsContactAsync)} - Ended call to Contact.Query Api.");

                return responseApi;
            }

            var failMessage = $"{nameof(ThereIsContactAsync)} - An error occurred when calling the Contact.Query Api. StatusCode: {response.StatusCode}";

            _logger.Error(failMessage);

            return output.Failure(failMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"{nameof(ThereIsContactAsync)} - An error occurred when calling the Contact.Query Api. Error: {ex.Message}";

            _logger.Error(errorMessage);

            return output.Failure(errorMessage);
        }
    }

}
