using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2CAJJ.Models;
using PracticaMvcCore2CAJJ.Repositories;

namespace PracticaMvcCore2CAJJ.ViewComponents
{
    public class MenuGenerosViewComponent : ViewComponent
    {
        private LibrosRepository repo;
        public MenuGenerosViewComponent(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
