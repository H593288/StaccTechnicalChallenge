using System.Globalization;
using System.Text.Json.Serialization;
using Flurl.Http;
using Newtonsoft.Json;

namespace StaccChallengeWebApp.Services;

public class KycApiService
{
    private readonly ILogger<KycApiService> _logger;

    public KycApiService(ILogger<KycApiService> logger)
    {
        _logger = logger;
    }
    // ? > this can be null 
    public async Task<IList<PepPersonModel>?> SearchPepListForPerson(string name)
    {
        var result = await $"https://code-challenge.stacc.dev/api/pep?name={name}".AllowAnyHttpStatus().GetAsync();

        if (result.StatusCode == 200)
        {
            var apiSearchResult = await result.GetJsonAsync<PepApiSearchResult>();
            if (apiSearchResult.Hits.Any())
            {
                return apiSearchResult.Hits.Select(hit => new PepPersonModel(
                    hit.Name,
                    ParseDate(hit.BirthDate), 
                    GetCountryNames(hit.Countries),
                    hit.Score,
                    hit.Phones,
                    hit.Emails
                    )
                ).ToList();
                    
            }
        }

        return default;
    }

    private DateOnly? ParseDate(string date)
    {
        if (!string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out DateOnly parseDateOnly))
        {
            return parseDateOnly;
        }

        return default;
    }

    private IList<string> GetCountryNames(string countryCodesString)
    {
        try
        {
            var countryCodes = countryCodesString.Split(";")
                .Select(countryCode => countryCode.Split("-")[0]);
            return countryCodes.Select(countryCode => new RegionInfo(countryCode).EnglishName).ToList();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "error");
        }

        return default;
    }


    public record PepApiSearchResult(
        int NumberOfHits,
        IList<PepApiSearchResultHit> Hits
    );

    public record PepApiSearchResultHit(
        double Score,
        string Id,
        string Schema,
        string Name,
        string Aliases,
        [property:JsonProperty("birth_date")]
        string BirthDate,
        string Countries,
        string Identifiers,
        string Sanctions,
        string Phones,
        string Emails,
        string Dataset,
        [property:JsonProperty("last_seen")]
        string LastSeen,
        [property:JsonProperty("first_seen")]
        string FirstSeen
    );

    public record PepPersonModel(string Name, DateOnly? Birthdate, IList<string> Countries, double Score, string Phones, string Emails);
    
    
}