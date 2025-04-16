using PracticeProject.Data;
using PracticeProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PracticeProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProjectController : Controller
    {
        private readonly ProjectData _projectData;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectController(ProjectData projectData, IWebHostEnvironment webHostEnvironment)
        {
            _projectData = projectData;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var projects = _projectData.GetProjects();
            return View(projects);
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProject(Projects project)
        {
            if (project.ImageFile != null)
            {
                // Save the uploaded image in wwwroot/images
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + project.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    project.ImageFile.CopyTo(fileStream);
                }

                // Save image path in the database
                project.ProjectImage = "/images/" + uniqueFileName;
            }

            _projectData.AddProject(project);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditProject(int id)
        {
            var project = _projectData.GetProjectsById(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }



        [HttpPost]
        public IActionResult EditProject(Projects project)
        {

            var existingProject = _projectData.GetProjectsById(project.ProjectId);

            if (existingProject == null)
            {
                return NotFound();
            }


            // Handle Image Upload
            if (project.ImageFile != null && project.ImageFile.Length > 0)
            {

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingProject.ProjectImage))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingProject.ProjectImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + project.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    project.ImageFile.CopyTo(fileStream);
                }

                project.ProjectImage = "/images/" + uniqueFileName;
            }
            else
            {
                project.ProjectImage = existingProject.ProjectImage;
            }

            // Update project
            _projectData.UpdateProject(project);

            return RedirectToAction("Index");
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int ProjectId)
        {
            if (ProjectId <= 0)
            {
                return BadRequest("Invalid Project ID");
            }

            _projectData.DeleteProject(ProjectId);
            return RedirectToAction("Index");
        }

    }
}