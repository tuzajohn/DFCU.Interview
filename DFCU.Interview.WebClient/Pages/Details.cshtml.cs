using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace DFCU.Interview.WebClient.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;
        public DetailsModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("DefaultClient");
        }

        [BindProperty(SupportsGet = true)]
        public Guid TransactionId { get; set; }

        public string? Status { get; set; } = string.Empty;
        public Transaction? TransactionDetails { get; private set; }
        public string? Message { get; set; }
        public async Task<IActionResult> OnGet(Guid id)
        {
            TransactionId = id;
            try
            {
                var response = await _httpClient.GetAsync($"api/payments/{id}/status");

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                    Message = errorResponse?.message ?? "An error occurred while processing your request.";
                    return Page();
                }
                else
                {
                    Status = await response.Content.ReadAsStringAsync();


                    return Page();
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        private Transaction? GetTransactionById(Guid id)
        {


            return new Transaction();
        }
    }
}
