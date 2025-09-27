using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Models;
using SchoolProject.Services;

namespace SchoolProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsService _newsService;

        public HomeController(ILogger<HomeController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5;

            var totalNews = await _newsService.GetTotalNewsCountAsync();
            var totalPages = (int)Math.Ceiling(totalNews / (double)pageSize);

            var news = await _newsService.GetPaginatedNewsAsync(page, pageSize);

            var vm = new NewsListViewModel
            {
                News = news,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(vm);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            News news = await _newsService.GetNewsByIdAsync(id);
            return View(news);
        }

        [HttpPost]
        public IActionResult Create(News model)
        {
            if (ModelState.IsValid)
            {
                _newsService.CreateNewsAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
