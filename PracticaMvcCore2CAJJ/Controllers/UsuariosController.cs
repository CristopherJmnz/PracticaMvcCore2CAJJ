using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using PracticaMvcCore2CAJJ.Repositories;

namespace PracticaMvcCore2CAJJ.Controllers
{
    public class UsuariosController : Controller
    {
        private LibrosRepository repo;
        public UsuariosController(LibrosRepository repo)
        {
            this.repo = repo;
        }
        [AuthorizeUsuario]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
