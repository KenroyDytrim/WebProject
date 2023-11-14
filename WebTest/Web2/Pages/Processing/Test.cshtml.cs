using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Octokit;
using Web2.Models;

namespace Web2.Pages
{
    public class TestModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public TestModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.Analyzes Analyzes { get; set; }
        [BindProperty]
        public Examination Examination { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id_a, int? id_e)
        {
            var client = new GitHubClient(new ProductHeaderValue("GitHub"), new Uri("https://github.com/"));
            client.Credentials = new Credentials("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");
            string username = "KenroyDytrim";
            string foldername = "PyScript";
            string filename = "Alldata2.csv";
            string branch = "main";
            var getfile = await client.Repository.Content.GetAllContentsByRef(username, foldername, filename, branch);

            var data = _context.analyzes.FromSqlRaw("SELECT * FROM analyzes ORDER by id_analysis").ToListAsync();
            string ls=getfile[0].Content;

            //for(int i=0; i<data.Result.Count(); i++)
            //{
            //    ls =ls + data.Result[i].serum_calcium.ToString().Replace(',', '.') + ',' + data.Result[i].serum_phosphorus.ToString().Replace(',', '.') + ',' + data.Result[i].serum_oxyproline.ToString().Replace(',', '.') + ',' + data.Result[i].calcium_excretion.ToString().Replace(',', '.') + ',' + data.Result[i].phosphorus_excretion.ToString().Replace(',', '.') + ','
            //        + data.Result[i].oxyproline_excretion.ToString().Replace(',', '.') + ',' + data.Result[i].severity_of_dst + ',' + "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0" + "\r\n";
            //}

            var updatefiless = await client.Repository.Content.UpdateFile(username, foldername, filename,
                new UpdateFileRequest("update file", ls, getfile.First().Sha));

            Analyzes = await _context.analyzes.FindAsync(id_a);
            Examination = await _context.examination.FindAsync(id_e);

            return Page();
         }
    }
}
