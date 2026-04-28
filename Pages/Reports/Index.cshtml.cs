using FastReport.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SchoolAppCore.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        // Property to hold the report
        public WebReport WebReport { get; set; }

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnGet()
        {
            // Register the MS SQL connection type
            FastReport.Utils.RegisteredObjects.AddConnection(typeof(FastReport.Data.MsSqlDataConnection));

            // Initialize the WebReport
            WebReport = new WebReport();

            // Path to your FastReport file (.frx)
            var reportPath = Path.Combine(_env.ContentRootPath, "Reports", "StudentGrades.frx");

            if (System.IO.File.Exists(reportPath))
            {
                WebReport.Report.Load(reportPath);
            }
        }
    }
}