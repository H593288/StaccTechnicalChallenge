using Microsoft.AspNetCore.Mvc.RazorPages;
using StaccChallengeWebApp.Services;

//backend
namespace StaccChallengeWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly KycApiService _kycApiService;

    public IndexModel(ILogger<IndexModel> logger, KycApiService kycApiService)
    {
        _logger = logger;
        _kycApiService = kycApiService;
    }

    public string SearchTerm { get; set; }

    public List<KycApiService.PepPersonModel> PepSearchResults { get; set; } = new();

    public async Task OnGet(string searchString)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            SearchTerm = searchString;
            var pepSearch = await _kycApiService.SearchPepListForPerson(searchString);
            if (pepSearch != null && pepSearch.Any()) 
                PepSearchResults.AddRange(pepSearch);
        }
        
    }
    
}