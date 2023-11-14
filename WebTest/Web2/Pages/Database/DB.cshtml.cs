using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web2.Pages
{
    public class DBModel : PageModel
    {
        private readonly ILogger<DBModel> _logger;

        public DBModel(ILogger<DBModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
          
        }

    }
}