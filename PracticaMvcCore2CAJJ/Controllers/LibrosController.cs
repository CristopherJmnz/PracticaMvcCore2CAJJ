using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using PracticaMvcCore2CAJJ.Extensions;
using PracticaMvcCore2CAJJ.Models;
using PracticaMvcCore2CAJJ.Repositories;

namespace PracticaMvcCore2CAJJ.Controllers
{
    public class LibrosController : Controller
    {
        private LibrosRepository repo;
        public LibrosController(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }

        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            List<Libro> libros = await this.repo.GetLibrosByGeneroAsync(idgenero);
            return View(libros);
        }

        public async Task<IActionResult> Details(int idlibro)
        {
            Libro libro = await this.repo.FindByIdAsync(idlibro);
            return View(libro);
        }

        public async Task<IActionResult> Carrito(int? ideliminar)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                if (ideliminar != null)
                {
                    carrito.Remove(ideliminar.Value);
                    if (carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("CARRITO", carrito);
                    }
                }
                List<Libro> libros= await this.repo.GetAllLibrosByIdAsync(carrito);
                return View(libros);
            }
            return View();
        }

        public IActionResult AddCarrito(int idlibro)
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito == null)
            {
                carrito = new List<int>();
                carrito.Add(idlibro);
            }
            else
            {
                carrito.Add(idlibro);
            }
            HttpContext.Session.SetObject("CARRITO", carrito);
            return RedirectToAction("Carrito");
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> Comprar()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Libro> libros = await this.repo.GetAllLibrosByIdAsync(carrito);
            int idFactura = await this.repo.GetMaxIdFacturaAsync();
            int idUser = int.Parse(HttpContext.User.FindFirst("ID").Value);
            foreach (Libro libro in libros)
            {
                await this.repo.ComprarAsync(libro.IdLibro,idUser,1,idFactura);
            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("Pedidos");
        }

        [AuthorizeUsuario]
        public async Task<IActionResult> Pedidos()
        {
            List<VistaPedidoView>listapedidos=  await this.repo.GetVistaPedidosAsync();
            return View(listapedidos);
        }
    }
}
