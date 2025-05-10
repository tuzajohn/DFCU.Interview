using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace DFCU.Interview.WebClient.Pages
{
    public class TransactionsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public TransactionsModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("DefaultClient");
        }

        [BindProperty]
        public required string Payer { get; set; }

        [BindProperty]
        [Display(Name = "Narration")]
        public string? PayerReference { get; set; }
        [BindProperty]
        public required string Payee { get; set; }
        [BindProperty]
        public required string Currency { get; set; }
        [BindProperty]
        public required decimal Amount { get; set; }

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Validation failed. Please correct the errors and try again.";
                return Page();
            }

            var transactionData = new
            {
                Payer,
                Payee,
                PayerReference,
                Currency,
                Amount
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(transactionData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/payments", jsonContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    var responseContext = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContext);

                    Message = errorResponse?.message ?? "An error occurred while processing your request.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
    public class ErrorResponse
    {
        public string? message { get; set; }
        public int statusCode { get; set; }
    }
}
