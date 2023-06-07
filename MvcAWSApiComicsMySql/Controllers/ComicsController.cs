using Microsoft.AspNetCore.Mvc;
using MvcAWSApiComicsMySql.Models;
using MvcAWSApiComicsMySql.Services;

namespace MvcAWSApiComicsMySql.Controllers
{
    public class ComicsController : Controller
    {
        private ServiceApiComics service;

        public ComicsController(ServiceApiComics service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Comic> comics = await this.service.GetComicsAsync();
            return View(comics);
        }

        public async Task<IActionResult> Details(int id)
        {
            Comic comic = await this.service.FindComicAsync(id);
            return View(comic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comic comic)
        {
            await this.service.CreateComic(comic);
            return RedirectToAction("Index");
        }

    }
}
