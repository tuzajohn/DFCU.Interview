using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DFCU.Interview.WebClient.Pages
{
    public class DetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid TransactionId { get; set; }

        public Transaction? TransactionDetails { get; private set; }

        public void OnGet(Guid id)
        {
            // Simulate fetching transaction details by ID  
            TransactionDetails = GetTransactionById(id);
        }

        private Transaction? GetTransactionById(Guid id)
        {


            return new Transaction();
        }
    }
}
