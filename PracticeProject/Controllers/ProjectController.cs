using Microsoft.AspNetCore.Mvc;
using PracticeProject.Data;

namespace PracticeProject.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectData _dataAccess;
      
        public ProjectController(ProjectData dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            var projects = _dataAccess.GetProjects();
            return View(projects);
        }
    }
}
