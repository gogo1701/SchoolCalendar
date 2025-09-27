using Microsoft.AspNetCore.Mvc;
using SchoolProject.Models;
using SchoolProject.Services;

namespace SchoolProject.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private NewsService _newsService;
        public ApiController(NewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCalendarItem()
        { 
            return Json(await _newsService.GetAllCalendarItemsAsync());
        }
    }
}
