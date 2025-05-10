using DFCU.Interview.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DFCU.Interview.WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public List<Transaction> Transactions { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("DefaultClient");
        }

        public async Task OnGetAsync()
        {

            Transactions = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/payments");

            _logger.LogInformation("Successfully retrieved {Count} payments", Transactions?.Count ?? 0);
        }
    }
    public class Transaction
    {
        public Guid Id { get; set; }
        public string Payer { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Narration { get; set; }
        public string? Currency { get; set; }
    }
}