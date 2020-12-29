using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepository _repository;

        public HomeController(ILogger<HomeController> logger,
                    IStudentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {

            // _repository.createStudent(new Student()
            // {
            //     Achternaam = "MR",
            //     Voornaam = "test"
            // });
            
            List<Student> students = new List<Student>();
            _logger.LogInformation("test123");
            students = _repository.getAllStudents().ToList();

            return View("Index",students);
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult ListItems()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}