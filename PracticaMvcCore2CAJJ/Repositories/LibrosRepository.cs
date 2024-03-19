using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2CAJJ.Data;
using PracticaMvcCore2CAJJ.Models;

namespace PracticaMvcCore2CAJJ.Repositories
{
    public class LibrosRepository
    {
        private LibrosContext context;
        public LibrosRepository(LibrosContext context)
        {
            this.context = context;
        }

        #region GENEROS

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        #endregion

        #region LIBROS

        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }

        public async Task<Libro> FindByIdAsync(int idLibro)
        {
            return await this.context.Libros
                .FirstOrDefaultAsync(x=>x.IdLibro==idLibro);
        }

        public async Task<List<Libro>> GetLibrosByGeneroAsync(int idgenero)
        {
            return await this.context.Libros
                .Where(x => x.IdGenero == idgenero).ToListAsync();
        }


        public async Task<List<Libro>> GetAllLibrosByIdAsync(List<int> listIdLibros)
        {
            var consulta = from datos in this.context.Libros
                           where listIdLibros.Contains(datos.IdLibro)
                           select datos;
            if (consulta.Count() == 0) return null;
            return await consulta.ToListAsync();
        }
        #endregion

        #region USUARIOS

        public async Task<Usuario>LoginEmpleadosAsync(string email, string password)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync
                (x => x.Email == email 
                && x.Password == password);
        }

        #endregion


        #region PEDIDOS

        public async Task<int> GetMaxIdPedidoAsync()
        {
            return await this.context.Pedidos.MaxAsync(x=>x.IdPedido) + 1;
        }

        public async Task<int> GetMaxIdFacturaAsync()
        {
            return (int)(await this.context.Pedidos.MaxAsync(x => x.IdFactura) + 1);
        }

        public async Task<List<VistaPedidoView>> GetVistaPedidosAsync()
        {
            return await this.context.VistaPedidoViews.ToListAsync();
        }

        public async Task ComprarAsync(int idLibro, int idUsuario,int cantidad, int idFactura)
        {
            Pedido ped = new Pedido
            {
                Cantidad=cantidad,
                Fecha=DateTime.Now,
                IdFactura=idFactura,
                IdLibro=idLibro,
                IdPedido= await this.GetMaxIdPedidoAsync(),
                IdUsuario=idUsuario
            };
            this.context.Add(ped);
            await this.context.SaveChangesAsync();
        }

        #endregion
    }
}
