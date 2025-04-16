using PracticeProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
namespace PracticeProject.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly AdminDataAccess _dataAccess;

        public AdminPanelController(AdminDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Username and Password are required";
                return View();
            }

            bool isValid = _dataAccess.ValidateAdmin(username, password);

            if (isValid)
            {
                HttpContext.Session.SetString("AdminUsername", username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Message = "Invalid Credentials";
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminUsername") == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["Layout"] = "~/Views/Shared/_LayoutAdminPanel.cshtml";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}

